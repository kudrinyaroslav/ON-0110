using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace TestTool.Tests.TestCases.TestSuites.AdvancedSecurity
{
    partial class AdvancedSecurityTestSuit
    {
        #region Entities

        abstract class X509CertificateBase
        {
            public abstract string SubjectDN { get; }

            public abstract string IssuerDN { get; }

            public abstract DateTime NotValidBefore { get; }

            public abstract DateTime NotValidAfter { get; }

            public abstract string SerialNumber { get; }

            public abstract byte[] PublicKey { get; }

            public abstract byte[] GetEncoded();
        }

        class X509CertificateBC: X509CertificateBase
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

		abstract class X509CertificateGeneratorBase
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

        class X509CertificateGeneratorBC: X509CertificateGeneratorBase
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

		class RSAKeyPair
		{
            public RSAKeyPair(byte[] publicKey, byte[] privateKey)
			{
			    PublicKey = publicKey;
			    PrivateKey = privateKey;
			}

            public byte[] PublicKey { get; protected set; }
            public byte[] PrivateKey { get; protected set; }
		}

        abstract class RSAKeyPairGeneratorBase
        {
            public abstract RSAKeyPair Generate();
        }

        class RSAKeyPairGeneratorBC: RSAKeyPairGeneratorBase
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

        abstract class CertificateSigningRequestBase
        {
            public abstract bool Verify();

            public abstract string SubjectDN { get; }

            public abstract byte[] PublicKey { get; }

            public abstract byte[] Extensions { get; }
        }

        class CertificateSigningRequestBC: CertificateSigningRequestBase
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
        #region Utils

        bool ValidateDEREncoding(Stream stream)
        {
            try
            {
                var asn1Stream = new Org.BouncyCastle.Asn1.Asn1InputStream(stream);
                var certificate = asn1Stream.ReadObject();

                var derEncodedStream = new MemoryStream();
                var encoder = new Org.BouncyCastle.Asn1.DerOutputStream(derEncodedStream);
                encoder.WriteObject(certificate);
                encoder.Flush();

                if (stream.Length != derEncodedStream.Length)
                    return false;

                stream.Seek(0, SeekOrigin.Begin);
                derEncodedStream.Seek(0, SeekOrigin.Begin);

                for (int i = 0; i < stream.Length; i++)
                {
                    if (stream.ReadByte() != derEncodedStream.ReadByte())
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

		bool X509NamesAreEqual(string l, string r)
		{
		    return new X509Name(l).Equivalent(new X509Name(r), false);
		}

        #endregion
    }
}
