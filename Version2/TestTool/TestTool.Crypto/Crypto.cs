using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509;

namespace TestTool.Crypto
{
    #region Entities

    public abstract class X509CertificateBase
    {
        public abstract string SubjectDN { get; }

        public abstract string IssuerDN { get; }

        public abstract DateTime NotValidBefore { get; }

        public abstract DateTime NotValidAfter { get; }

        public abstract string SerialNumber { get; }

        public abstract byte[] PublicKey { get; }

        public abstract byte[] GetEncoded();
    }

    public class X509CertificateBC : X509CertificateBase
    {
        public X509CertificateBC(Stream stream)
        {
            var parser = new X509CertificateParser();
            m_Certificate = parser.ReadCertificate(stream);
        }

        public override string SubjectDN
        {
            get { return m_Certificate.SubjectDN.ToString(); }
        }

        public override string IssuerDN
        {
            get { return m_Certificate.IssuerDN.ToString(); }
        }

        public override DateTime NotValidBefore
        {
            get { return m_Certificate.NotBefore; }
        }

        public override DateTime NotValidAfter
        {
            get { return m_Certificate.NotAfter; }
        }

        public override string SerialNumber
        {
            get { return m_Certificate.SerialNumber.ToString(); }
        }

        public override byte[] PublicKey
        {
            get { return SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(m_Certificate.GetPublicKey()).GetDerEncoded(); }
        }

        public override byte[] GetEncoded()
        {
            return m_Certificate.GetEncoded();
        }

        private readonly Org.BouncyCastle.X509.X509Certificate m_Certificate;
    }

    public abstract class X509CertificateGeneratorBase
    {
        public abstract void SetSubjectDN(string subject);

        public abstract void SetIssuerDN(string issuer);

        public abstract void SetNotValidBefore(DateTime T);

        public abstract void SetNotValidAfter(DateTime T);

        public abstract void SetSerialNumber(string sn);

        public abstract void SetSignatureAlgorithm(string sa);

        public abstract void SetPublicKey(byte[] pk);

        public abstract void CopyExtensions(byte[] stream);

        public abstract X509CertificateBase Generate(byte[] privateKey);
    }

    public class X509CertificateGeneratorBC : X509CertificateGeneratorBase
    {
        public X509CertificateGeneratorBC()
        {
            m_Generator = new X509V3CertificateGenerator();
        }

        public override void SetSubjectDN(string subject)
        {
            m_Generator.SetSubjectDN(new X509Name(subject));
        }

        public override void SetIssuerDN(string issuer)
        {
            m_Generator.SetIssuerDN(new X509Name(issuer));
        }

        public override void SetNotValidBefore(DateTime T)
        {
            m_Generator.SetNotBefore(T);
        }

        public override void SetNotValidAfter(DateTime T)
        {
            m_Generator.SetNotAfter(T);
        }

        public override void SetSerialNumber(string sn)
        {
            m_Generator.SetSerialNumber(new BigInteger(sn));
        }

        public override void SetSignatureAlgorithm(string sa)
        {
            m_Generator.SetSignatureAlgorithm(sa);
        }

        public void SetSerialNumber()
        {
            SetSerialNumber(BigInteger.ValueOf(DateTime.Now.Ticks).ToString());
        }

        public override void SetPublicKey(byte[] pk)
        {
            m_Generator.SetPublicKey(PublicKeyFactory.CreateKey(pk));
        }

        public override void CopyExtensions(byte[] extensions)
        {
            var e = X509Extensions.GetInstance(extensions);
            if (null != e)
                foreach (DerObjectIdentifier oid in e.ExtensionOids)
                {
                    var extension = e.GetExtension(oid);
                    m_Generator.AddExtension(oid, extension.IsCritical, extension.GetParsedValue());
                }
        }

        public override X509CertificateBase Generate(byte[] privateKey)
        {
            var certificate = m_Generator.Generate(PrivateKeyFactory.CreateKey(privateKey)).GetEncoded();
            return new X509CertificateBC(new MemoryStream(certificate));
        }

        private readonly X509V3CertificateGenerator m_Generator;
    }

    public class RSAKeyPair
    {
        public RSAKeyPair(byte[] publicKey, byte[] privateKey, bool isEncrypted = false)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
            IsEncrypted = isEncrypted;
        }

        public bool IsEncrypted { get; protected set; }

        public byte[] PublicKey { get; protected set; }
        public byte[] PrivateKey { get; protected set; }

        public static RSAKeyPair Encrypt(string passphrase, RSAKeyPair keyPair)
        {
            var salt = new byte[20];
            new SecureRandom().NextBytes(salt);

            return new RSAKeyPair(keyPair.PublicKey,
                                  EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo(PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc.Id,
                                                                                               passphrase.ToArray(),
                                                                                               salt,
                                                                                               1024,
                                                                                               PrivateKeyInfo.GetInstance(keyPair.PrivateKey)).GetDerEncoded(),
                                  true);
        }
    }

    public abstract class RSAKeyPairGeneratorBase
    {
        public abstract RSAKeyPair Generate();
    }

    public class RSAKeyPairGeneratorBC : RSAKeyPairGeneratorBase
    {
        public RSAKeyPairGeneratorBC(int strength)
        {
            m_Generator = new RsaKeyPairGenerator();
            m_Generator.Init(new KeyGenerationParameters(new SecureRandom(), strength));
        }

        public override RSAKeyPair Generate()
        {
            var r = m_Generator.GenerateKeyPair();

            return new RSAKeyPair(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(r.Public).GetDerEncoded(),
                                  PrivateKeyInfoFactory.CreatePrivateKeyInfo(r.Private).GetDerEncoded());
        }

        private readonly RsaKeyPairGenerator m_Generator;
    }

    public class PKCS12KeyStore
    {
        public PKCS12KeyStore(string keyAlias, RSAKeyPair keyPair, X509CertificateBase certificate)
        {
            m_KeyStore = new Pkcs12StoreBuilder().SetUseDerEncoding(true).Build();

            m_KeyStore.SetKeyEntry(keyAlias,
                                   new AsymmetricKeyEntry(PrivateKeyFactory.CreateKey(keyPair.PrivateKey)),
                                   new[] { new X509CertificateEntry(new X509CertificateParser().ReadCertificate(certificate.GetEncoded())) });

        }

        public void Save(Stream stream, string encryptionPassword, string integrityPassword)
        {
            m_KeyStore.Save(stream, encryptionPassword, integrityPassword, new SecureRandom());
        }

        private readonly Pkcs12Store m_KeyStore;
    }

    #region CSR

    public abstract class CertificateSigningRequestBase
    {
        public abstract bool Verify();

        public abstract string SubjectDN { get; }

        public abstract byte[] PublicKey { get; }

        public abstract byte[] Extensions { get; }
    }

    public class CertificateSigningRequestBC : CertificateSigningRequestBase
    {
        public CertificateSigningRequestBC(Stream stream)
        {
            m_Request = new Pkcs10CertificationRequest(new Asn1InputStream(stream).ReadObject() as Asn1Sequence);
        }

        public override bool Verify()
        {
            return m_Request.Verify();
        }

        public override string SubjectDN
        {
            get { return m_Request.GetCertificationRequestInfo().Subject.ToString(); }
        }

        public override byte[] PublicKey
        {
            get { return m_Request.GetCertificationRequestInfo().SubjectPublicKeyInfo.GetDerEncoded(); }
        }

        public override byte[] Extensions
        {
            get
            {
                if (null != m_Request.GetCertificationRequestInfo().Attributes)
                    foreach (DerSequence attribute in m_Request.GetCertificationRequestInfo().Attributes.OfType<DerSequence>())
                    {
                        var a = AttributePkcs.GetInstance(attribute);
                        if (a.AttrType.Equals(PkcsObjectIdentifiers.Pkcs9AtExtensionRequest) && a.AttrValues.Count > 0)
                        {
                            var extensions = X509Extensions.GetInstance(a.AttrValues.OfType<Asn1Sequence>().FirstOrDefault());
                            if (null != extensions)
                                extensions.GetDerEncoded();
                        }
                    }

                return null;
            }
        }

        private readonly Pkcs10CertificationRequest m_Request;
    }

    #endregion

    #region CRL
    public abstract class CertificateRevocationListBase
    {
        public abstract string SignatureAlgorithm { get; }

        public abstract string IssuerDN { get; }

        public abstract DateTime ThisUpdate { get; }

        public abstract DateTime? NextUpdate { get; }

        public abstract byte[] Signature { get; }

        public abstract byte[] GetEncoded();
    }

    public class CertificateRevocationListBC : CertificateRevocationListBase
    {
        public CertificateRevocationListBC(Stream stream)
        {
            m_CRL = (new X509CrlParser()).ReadCrl(stream);
        }

        public override string SignatureAlgorithm
        {
            get { return m_CRL.SigAlgOid; }
        }

        public override string IssuerDN
        {
            get { return m_CRL.IssuerDN.ToString(); }
        }

        public override DateTime ThisUpdate
        {
            get { return m_CRL.ThisUpdate; }
        }

        public override DateTime? NextUpdate
        {
            get { return (null == m_CRL.NextUpdate) ? (DateTime?)null : m_CRL.NextUpdate.Value; }
        }

        public override byte[] Signature
        {
            get { return m_CRL.GetSignature(); }
        }

        public override byte[] GetEncoded()
        { return m_CRL.GetEncoded(); }

        private readonly X509Crl m_CRL;
    }


    public abstract class CertificateRevocationListGeneratorBase
    {
        public abstract void SetSignatureAlgorithm(string signatureAlgorithm);

        public abstract void SetIssuerDN(string issuerDN);

        public abstract void SetThisUpdate(DateTime date);

        public abstract void SetNextUpdate(DateTime date);

        public abstract void AddCRLEntry(string certificateNumber, DateTime revocationDate);

        public abstract CertificateRevocationListBase Generate(RSAKeyPair keyPair);
    }

    public class CertificateRevocationListGeneratorBC : CertificateRevocationListGeneratorBase
    {
        private X509V2CrlGenerator m_Generator = new X509V2CrlGenerator();

        public override void SetSignatureAlgorithm(string signatureAlgorithm)
        {
            m_Generator.SetSignatureAlgorithm(signatureAlgorithm);
        }

        public override void SetIssuerDN(string issuerDN)
        {
            m_Generator.SetIssuerDN(new X509Name(issuerDN));
        }

        public override void SetThisUpdate(DateTime date)
        {
            m_Generator.SetThisUpdate(date);
        }

        public override void SetNextUpdate(DateTime date)
        {
            m_Generator.SetNextUpdate(date);
        }

        public override void AddCRLEntry(string certificateNumber, DateTime revocationDate)
        {
            m_Generator.AddCrlEntry(new BigInteger(certificateNumber), revocationDate, 0);
        }

        public override CertificateRevocationListBase Generate(RSAKeyPair keyPair)
        {
            var r = m_Generator.Generate(PrivateKeyFactory.CreateKey(keyPair.PrivateKey));

            return new CertificateRevocationListBC(new MemoryStream(r.GetEncoded()));
        }
    }
    #endregion

    #endregion

    public static class CryptoUtils
    {
        public static bool CheckCryptoLibary()
        {
            try
            { AppDomain.CurrentDomain.Load(new AssemblyName("BouncyCastle.Crypto")); }
            catch
            { return false; }

            return true;
        }

        public static void Save(this Pkcs12Store store,
                                Stream stream,
                                string encryptionPassword,
                                string integrityPassword,
                                SecureRandom random)
        {
            const int saltSize = 20;
            const int minIterations = 1024;

            if (stream == null)
                throw new ArgumentNullException("stream");
            //if (null != encryptionPassword && encryptionPassword == integrityPassword)
            //{
            //    store.Save(stream, encryptionPassword.ToArray(), random);
            //    return;
            //}
            if (random == null)
                throw new ArgumentNullException("random");

            var T = store.GetType();
            Func<AsymmetricKeyParameter, SubjectKeyIdentifier> CreateSubjectKeyID = (pubKey_) =>
            {
                var method = T.GetMethod("CreateSubjectKeyID", BindingFlags.NonPublic | BindingFlags.Static);
                return (SubjectKeyIdentifier)method.Invoke(store, new object[] { pubKey_ });
            };

            Func<DerObjectIdentifier> keyAlgorithm = () =>
            {
                var property = T.GetField("keyAlgorithm", BindingFlags.NonPublic | BindingFlags.Instance);
                return (DerObjectIdentifier)property.GetValue(store);
            };


            Func<DerObjectIdentifier> certAlgorithm = () =>
            {
                var property = T.GetField("certAlgorithm", BindingFlags.NonPublic | BindingFlags.Instance);
                return (DerObjectIdentifier)property.GetValue(store);
            };
            //
            // handle the key
            //
            Asn1EncodableVector keyS = new Asn1EncodableVector();
            var keys = store.Aliases.OfType<string>().ToDictionary(alias => alias, store.GetKey);
            foreach (string name in store.Aliases.OfType<string>())
            {
                byte[] kSalt = new byte[saltSize];
                random.NextBytes(kSalt);

                AsymmetricKeyEntry privKey = keys[name];
                Asn1Encodable kInfo = null;
                if (!string.IsNullOrEmpty(encryptionPassword))
                    kInfo = EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo(keyAlgorithm(), encryptionPassword.ToArray(), kSalt, minIterations, privKey.Key);
                else 
                    kInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privKey.Key);

                Asn1EncodableVector kName = new Asn1EncodableVector();

                foreach (string oid in privKey.BagAttributeKeys)
                {
                    Asn1Encodable entry = privKey[oid];

                    // NB: Ignore any existing FriendlyName
                    if (oid.Equals(PkcsObjectIdentifiers.Pkcs9AtFriendlyName.Id))
                        continue;

                    kName.Add(new DerSequence(new DerObjectIdentifier(oid), new DerSet(entry)));
                }

                //
                // make sure we are using the local alias on store
                //
                // NB: We always set the FriendlyName based on 'name'
                //if (privKey[PkcsObjectIdentifiers.Pkcs9AtFriendlyName] == null)
                {
                    kName.Add(new DerSequence(PkcsObjectIdentifiers.Pkcs9AtFriendlyName, new DerSet(new DerBmpString(name))));
                }

                //
                // make sure we have a local key-id
                //
                if (privKey[PkcsObjectIdentifiers.Pkcs9AtLocalKeyID] == null)
                {
                    X509CertificateEntry ct = store.GetCertificate(name);
                    AsymmetricKeyParameter pubKey = ct.Certificate.GetPublicKey();
                    SubjectKeyIdentifier subjectKeyID = CreateSubjectKeyID(pubKey);

                    kName.Add(new DerSequence(PkcsObjectIdentifiers.Pkcs9AtLocalKeyID, new DerSet(subjectKeyID)));
                }

                SafeBag kBag = null;
                if (!string.IsNullOrEmpty(encryptionPassword)) 
                    kBag = new SafeBag(PkcsObjectIdentifiers.Pkcs8ShroudedKeyBag, kInfo.ToAsn1Object(), new DerSet(kName));
                else
                    kBag = new SafeBag(PkcsObjectIdentifiers.KeyBag, kInfo.ToAsn1Object(), new DerSet(kName));
                keyS.Add(kBag);
            }

            byte[] derEncodedBytes = new DerSequence(keyS).GetDerEncoded();

            BerOctetString keyString = new BerOctetString(derEncodedBytes);

            //
            // certificate processing
            //
            byte[] cSalt = new byte[saltSize];

            random.NextBytes(cSalt);

            Asn1EncodableVector certSeq = new Asn1EncodableVector();
            Pkcs12PbeParams cParams = new Pkcs12PbeParams(cSalt, minIterations);
            AlgorithmIdentifier cAlgId = new AlgorithmIdentifier(certAlgorithm(), cParams.ToAsn1Object());
            ISet doneCerts = new HashSet();

            foreach (string name in keys.Keys)
            {
                X509CertificateEntry certEntry = store.GetCertificate(name);
                CertBag cBag = new CertBag(PkcsObjectIdentifiers.X509Certificate, new DerOctetString(certEntry.Certificate.GetEncoded()));

                Asn1EncodableVector fName = new Asn1EncodableVector();

                foreach (string oid in certEntry.BagAttributeKeys)
                {
                    Asn1Encodable entry = certEntry[oid];

                    // NB: Ignore any existing FriendlyName
                    if (oid.Equals(PkcsObjectIdentifiers.Pkcs9AtFriendlyName.Id))
                        continue;

                    fName.Add(new DerSequence(new DerObjectIdentifier(oid), new DerSet(entry)));
                }

                //
                // make sure we are using the local alias on store
                //
                // NB: We always set the FriendlyName based on 'name'
                //if (certEntry[PkcsObjectIdentifiers.Pkcs9AtFriendlyName] == null)
                {
                    fName.Add(new DerSequence(PkcsObjectIdentifiers.Pkcs9AtFriendlyName, new DerSet(new DerBmpString(name))));
                }

                //
                // make sure we have a local key-id
                //
                if (certEntry[PkcsObjectIdentifiers.Pkcs9AtLocalKeyID] == null)
                {
                    AsymmetricKeyParameter pubKey = certEntry.Certificate.GetPublicKey();
                    SubjectKeyIdentifier subjectKeyID = CreateSubjectKeyID(pubKey);

                    fName.Add(new DerSequence(PkcsObjectIdentifiers.Pkcs9AtLocalKeyID, new DerSet(subjectKeyID)));
                }

                SafeBag sBag = new SafeBag(PkcsObjectIdentifiers.CertBag, cBag.ToAsn1Object(), new DerSet(fName));

                certSeq.Add(sBag);

                doneCerts.Add(certEntry.Certificate);
            }

            var certs = store.Aliases.OfType<string>().Select(store.GetCertificate);
            foreach (var cert in certs)
            {
                //X509CertificateEntry cert = (X509CertificateEntry)certs[certId];

                //if (keys[certId] != null)
                //    continue;
                if (doneCerts.Contains(cert.Certificate))
                    continue;

                CertBag cBag = new CertBag(PkcsObjectIdentifiers.X509Certificate, new DerOctetString(cert.Certificate.GetEncoded()));

                Asn1EncodableVector fName = new Asn1EncodableVector();

                foreach (string oid in cert.BagAttributeKeys)
                {
                    // a certificate not immediately linked to a key doesn't require
                    // a localKeyID and will confuse some PKCS12 implementations.
                    //
                    // If we find one, we'll prune it out.
                    if (oid.Equals(PkcsObjectIdentifiers.Pkcs9AtLocalKeyID.Id))
                        continue;

                    Asn1Encodable entry = cert[oid];

                    // NB: Ignore any existing FriendlyName
                    if (oid.Equals(PkcsObjectIdentifiers.Pkcs9AtFriendlyName.Id))
                        continue;

                    fName.Add(new DerSequence(new DerObjectIdentifier(oid), new DerSet(entry)));
                }

                //
                // make sure we are using the local alias on store
                //
                // NB: We always set the FriendlyName based on 'certId'
                //if (cert[PkcsObjectIdentifiers.Pkcs9AtFriendlyName] == null)
                {
                    //fName.Add(new DerSequence(PkcsObjectIdentifiers.Pkcs9AtFriendlyName, new DerSet(new DerBmpString(certId))));
                    fName.Add(new DerSequence(PkcsObjectIdentifiers.Pkcs9AtFriendlyName, new DerSet(new DerBmpString(CreateSubjectKeyID(cert.Certificate.GetPublicKey()).GetKeyIdentifier()))));
                }

                SafeBag sBag = new SafeBag(PkcsObjectIdentifiers.CertBag, cBag.ToAsn1Object(), new DerSet(fName));

                certSeq.Add(sBag);

                doneCerts.Add(cert.Certificate);
            }

            var chainCerts = store.Aliases.OfType<string>().Select(store.GetCertificateChain).Aggregate<IEnumerable<X509CertificateEntry>, IEnumerable<X509CertificateEntry>>(new List<X509CertificateEntry>(), (list, entries) => list.Union(entries));
            foreach (var cert in chainCerts)
            {
                //X509CertificateEntry cert = (X509CertificateEntry)chainCerts[certId];

                if (doneCerts.Contains(cert.Certificate))
                    continue;

                CertBag cBag = new CertBag(PkcsObjectIdentifiers.X509Certificate, new DerOctetString(cert.Certificate.GetEncoded()));

                Asn1EncodableVector fName = new Asn1EncodableVector();

                foreach (string oid in cert.BagAttributeKeys)
                {
                    // a certificate not immediately linked to a key doesn't require
                    // a localKeyID and will confuse some PKCS12 implementations.
                    //
                    // If we find one, we'll prune it out.
                    if (oid.Equals(PkcsObjectIdentifiers.Pkcs9AtLocalKeyID.Id))
                        continue;

                    fName.Add(new DerSequence(new DerObjectIdentifier(oid), new DerSet(cert[oid])));
                }

                SafeBag sBag = new SafeBag(PkcsObjectIdentifiers.CertBag, cBag.ToAsn1Object(), new DerSet(fName));

                certSeq.Add(sBag);
            }

            derEncodedBytes = new DerSequence(certSeq).GetDerEncoded();

            Func<bool, AlgorithmIdentifier, char[], bool, byte[], byte[] > CryptPbeData = (forEncryption_, algId_, password_, wrongPkcs12Zero_, data_) =>
                {
                    var method = T.GetMethod("CryptPbeData", BindingFlags.NonPublic | BindingFlags.Static);
                    return (byte[])method.Invoke(store, new object[] { forEncryption_, algId_, password_, wrongPkcs12Zero_, data_ });
                };

            ContentInfo[] info = null;
            if (null != encryptionPassword)
            {
                byte[] certBytes = CryptPbeData(true, cAlgId, encryptionPassword.ToArray(), false, derEncodedBytes);

                var cInfo = new EncryptedData(PkcsObjectIdentifiers.Data, cAlgId, new BerOctetString(certBytes));

                info = new ContentInfo[]
                    {
                        new ContentInfo(PkcsObjectIdentifiers.Data, keyString),
                        new ContentInfo(PkcsObjectIdentifiers.EncryptedData, cInfo.ToAsn1Object())
                    };
            }
            else
            {
                var cInfo = new BerOctetString(derEncodedBytes);

                info = new ContentInfo[]
                    {
                        new ContentInfo(PkcsObjectIdentifiers.Data, keyString),
                        new ContentInfo(PkcsObjectIdentifiers.Data, cInfo.ToAsn1Object())
                    };
            }

            byte[] data = new AuthenticatedSafe(info).GetEncoded(Asn1Encodable.Der);

            ContentInfo mainInfo = new ContentInfo(PkcsObjectIdentifiers.Data, new BerOctetString(data));

            //
            // create the mac
            //
            byte[] mSalt = new byte[saltSize];
            random.NextBytes(mSalt);

            Func<DerObjectIdentifier, byte[], int, char[], bool, byte[], byte[]> CalculatePbeMac = (oid_, salt_, itCount_, password_, wrongPkcs12Zero_, data_) =>
                {
                    var method = T.GetMethod("CalculatePbeMac", BindingFlags.NonPublic | BindingFlags.Static);
                    return (byte[])method.Invoke(store, new object[] { oid_, salt_, itCount_, password_, wrongPkcs12Zero_, data_ });
                };


            MacData mData = null;

            if (null != integrityPassword)
            {
                //byte[] mac = CalculatePbeMac(OiwObjectIdentifiers.IdSha1, mSalt, minIterations, integrityPassword.ToArray(), false, data);
                byte[] mac = CalculatePbeMac(PbeUtilities.GetObjectIdentifier("PBEwithHmacSHA-256"), mSalt, minIterations, integrityPassword.ToArray(), false, data);

                //AlgorithmIdentifier algId = new AlgorithmIdentifier(OiwObjectIdentifiers.IdSha1, DerNull.Instance);
                AlgorithmIdentifier algId = new AlgorithmIdentifier(PbeUtilities.GetObjectIdentifier("PBEwithHmacSHA-256"), DerNull.Instance);
                
                DigestInfo dInfo = new DigestInfo(algId, mac);

                mData = new MacData(dInfo, mSalt, minIterations);
            }

            //
            // output the Pfx
            //
            Pfx pfx = new Pfx(mainInfo, mData);

            DerOutputStream derOut = new DerOutputStream(stream);

            derOut.WriteObject(pfx);
        }

        public static byte[] SignCHUIDByRandomKeyPair(IEnumerable<byte> value)
        {
            var keyPair = new RSAKeyPairGeneratorBC(1024).Generate();

            var certificateGenerator = new X509CertificateGeneratorBC();
            certificateGenerator.SetIssuerDN("CN=ONVIF TT, C=US");
            certificateGenerator.SetSubjectDN("CN=ONVIF TT, C=US");
            certificateGenerator.SetNotValidBefore(DateTime.UtcNow.AddYears(-1));
            certificateGenerator.SetNotValidAfter(DateTime.UtcNow.AddYears(1));
            certificateGenerator.SetSerialNumber();
            certificateGenerator.SetSignatureAlgorithm("SHA1WithRSAEncryption");
            certificateGenerator.SetPublicKey(keyPair.PublicKey);

            var generator = new CmsSignedDataGenerator();
            generator.AddSigner(PrivateKeyFactory.CreateKey(keyPair.PrivateKey), 
                                certificateGenerator.Generate(keyPair.PrivateKey).GetEncoded(),
                                CmsSignedDataGenerator.DigestSha1);

            return generator.Generate(new CmsProcessableByteArray(value.ToArray())).GetEncoded();
        }
    }

    #region PKCS#8

    public class OneAsymmetricKeyInfo: PrivateKeyInfo
    {
        public OneAsymmetricKeyInfo(AlgorithmIdentifier algID, Asn1Object privateKey): this(algID, privateKey, null)
        {}

        public OneAsymmetricKeyInfo(AlgorithmIdentifier algID, Asn1Object privateKey, Asn1Set attributes): this(algID, privateKey, attributes, null)
        {}

        public OneAsymmetricKeyInfo(AlgorithmIdentifier algID, Asn1Object privateKey, Asn1Object publicKey): this(algID, privateKey, null, publicKey)
        {}

        public OneAsymmetricKeyInfo(AlgorithmIdentifier algID, Asn1Object privateKey, Asn1Set attributes, Asn1Object publicKey): base(algID, privateKey, attributes)
        {
            m_PublicKey = publicKey;
        }

        public override Asn1Object ToAsn1Object()
        {
			var v = new Asn1EncodableVector(new DerInteger(null == PublicKey ? 0 : 1), AlgorithmID, new DerOctetString(PrivateKey));

			if (Attributes != null)
				v.Add(new DerTaggedObject(false, 0, Attributes));

            if (null != PublicKey)
                v.Add(PublicKey);

			return new DerSequence(v);
        }

        public Asn1Object PublicKey
        {
            get { return m_PublicKey; }
        }

        private readonly Asn1Object m_PublicKey;
    }

    public sealed class OneAsymmetricKeyInfoFactory
    {
        private OneAsymmetricKeyInfoFactory()
        {}

        public static OneAsymmetricKeyInfo CreateOneAsymmetricKeyInfo(AsymmetricKeyParameter privateKey)
        {
            var r = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);

            return new OneAsymmetricKeyInfo(r.AlgorithmID, r.PrivateKey, r.Attributes);
        }

        public static OneAsymmetricKeyInfo CreateOneAsymmetricKeyInfo(PrivateKeyInfo privateKey,
                                                                      SubjectPublicKeyInfo publicKey)
        {
            return new OneAsymmetricKeyInfo(privateKey.AlgorithmID, privateKey.PrivateKey, privateKey.Attributes, publicKey.PublicKeyData);
        }

        public static OneAsymmetricKeyInfo CreateOneAsymmetricKeyInfo(AsymmetricKeyParameter privateKey, AsymmetricKeyParameter publicKey)
        {
			if (publicKey.IsPrivate)
				throw new ArgumentException("Private key passed - public key expected", "publicKey");

            return CreateOneAsymmetricKeyInfo(PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey), SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey));
        }

        public static OneAsymmetricKeyInfo CreateOneAsymmetricKeyInfo(RSAKeyPair keyPair)
        {
            return CreateOneAsymmetricKeyInfo(PrivateKeyInfo.GetInstance(keyPair.PrivateKey), SubjectPublicKeyInfo.GetInstance(keyPair.PublicKey));
        }
    }

    #endregion
}
