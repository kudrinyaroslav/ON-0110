using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestTool.Tests.TestCases.Utils.IBaseOnvifService;

namespace TestTool.Tests.TestCases.TestSuites.Credential_
{
    class CredentialIdentifierValueFactory
    {
        protected static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);

            return ret;
        }

        protected static byte[] WiegandValue(int size, IEnumerable<byte> value)
        {
            var r = new BitArray(size + 2, false);

            for (int i = 0; i < size; ++i)
                r[i + 1] = (0 != (value.ElementAt(i/8) & (1 << (i % 8))));

            var partSize = size/2 + size % 2;

            var firstParity = 0;
            for (int i = 0; i < partSize; ++i)
                firstParity += r[i + 1] ? 1 : 0;
            r[0] = (1 == (firstParity % 2));

            var secondParity = 0;
            for (int i = 0; i < partSize; ++i)
                secondParity += r[partSize + i - size % 2] ? 1 : 0;
            r[size + 1] = (0 == (secondParity % 2));

            return BitArrayToByteArray(r);            
        }

        protected static byte[] Wiegand26(byte facilityCode, UInt16 cardNumber)
        {
            var value = new byte[4];
            value[0] = facilityCode;
            value[1] = (byte)(cardNumber & 0xFF);
            value[2] = (byte)((cardNumber >> 8) & 0xFF);

            return WiegandValue(24, value);
        }

        protected static byte[] Wiegand37WithoutFacilityCode(IEnumerable<byte> cardNumber)
        {
            if (5 != cardNumber.Count())
                throw new ArgumentException("cardNumber: array should be of size 5");

            return WiegandValue(35, cardNumber);
        }

        protected static byte[] Wiegand37WithFacilityCode(UInt16 facilityCode, byte[] cardNumber)
        {
            if (3 != cardNumber.Count())
                throw new ArgumentException("cardNumber: array should be of size 3");

            var value = new byte[5];
            value[0] = (byte)(facilityCode & 0xFF);
            value[1] = (byte)((facilityCode >> 8) & 0xFF);
            value[2] = cardNumber[0];
            value[3] = cardNumber[1];
            value[4] = cardNumber[2];

            return WiegandValue(35, value);
        }

        protected static byte[] Wiegand50WithFacilityCode(UInt16 facilityCode, byte[] cardNumber)
        {
            if (4 != cardNumber.Count())
                throw new ArgumentException("cardNumber: array should be of size 4");

            var value = new byte[6];
            value[0] = (byte)(facilityCode & 0xFF);
            value[1] = (byte)((facilityCode >> 8) & 0xFF);
            for (int i = 0; i < 4; ++i)
                value[2 + i] = cardNumber[i];

            return WiegandValue(48, value);
        }

        protected static byte[] Wiegand66WithFacilityCode(UInt32 facilityCode, byte[] cardNumber)
        {
            if (4 != cardNumber.Count())
                throw new ArgumentException("cardNumber: array should be of size 4");

            var value = new byte[8];
            for (int i = 0; i < 4; ++i)
                value[i] = (byte)((facilityCode >> i*8) & 0xFF);
            for (int i = 0; i < 4; ++i)
                value[4 + i] = cardNumber[i];

            return WiegandValue(64, value);
        }

        protected static byte[] SimpleAlphaNumeric(byte characterSet, byte[] value)
        {
            var valueSize = null != value ? value.Count() : 0;

            var size = 1 + valueSize;

            if (size > 255)
                throw new ArgumentException("value: array should be of size less or equal to 254");

            var r = new byte[1 + size];

            r[0] = (byte) size;
            r[1] = characterSet;
            for (int i = 0; i < valueSize; ++i)
                r[2 + i] = value[i];

            return r;
        }

        protected static byte[] SimpleNumber(int size, byte[] cardNumber)
        {
            var r = new byte[(size - 1)/8 + 1];
            for (int i = 0; i < r.Count(); ++i)
                r[i] = 0;

            for (int i = 0; i < size; ++i)
                r[i/8] |= (byte)(cardNumber[i/8] & (1 << (i % 8)));

            return r;
        }

        protected static byte[] GUID(IEnumerable<byte> value)
        {
            if (null != value && 16 < value.Count())
                throw new ArgumentException("value: array should be of size less or equal to 16");

            var r = new byte[16];

            if (null != value)
                for (int i = 0; i < value.Count(); i++)
                    r[i] = value.ElementAt(i);

            for (int i = null != value ? value.Count() : 0; i < r.Count(); i++)
                r[i] = 0;

            return r;
        }

        protected static byte[] UsernamePassword(byte usernameCharacterSet, byte[] username, byte passwordCharacterSet, byte[] password)
        {
            var usernameSize = (null != username) ? username.Count() : 0;
            if (null != username && usernameSize >= 254)
                throw new ArgumentException("username: array should be of size less or equal to 254");
            
            var passwordSize = (null != password) ? password.Count() : 0;
            if (null != password && passwordSize >= 254)
                throw new ArgumentException("password: array should be of size less or equal to 254");

            var size = 1 + usernameSize + 1 + passwordSize;
            var r = new byte[1 + 1 + size];

            r[0] = (byte)(1 + usernameSize);
            r[1] = usernameCharacterSet;
            for (int i = 0; i < usernameSize; i++)
                r[2 + i] = username[i];

            r[2 + usernameSize] = (byte)(1 + passwordSize);
            r[3 + usernameSize] = passwordCharacterSet;
            for (int i = 0; i < passwordSize; i++)
                r[4 + usernameSize + i] = password[i];

            return r;
        }

        protected static byte[] FASCN(UInt16 agencyCode, UInt16 systemSiteCode, UInt32 credentialNumber)
        {
            return BitConverter.GetBytes(agencyCode)
                               .Concat(BitConverter.GetBytes(systemSiteCode))
                               .Concat(BitConverter.GetBytes(credentialNumber))
                               .ToArray();
        }

        protected static byte[] FASCNLarge(UInt16 agencyCode, UInt16 systemSiteCode, UInt32 credentialNumber,
                                           byte seriesCode, byte credentialCode,
                                           IEnumerable<byte> personalID, 
                                           byte organizationalCategory, UInt16 organizationalID,
                                           byte associationCategory)
        {
            if (null == personalID)
                throw new ArgumentNullException("personalID");

            if (5 != personalID.Count())
                throw new ArgumentException("personalID: array should be of size 5(40 bits)");

            return FASCN(agencyCode, systemSiteCode, credentialNumber).Concat(new [] { seriesCode })
                                                                      .Concat(new [] { credentialCode })
                                                                      .Concat(personalID)
                                                                      .Concat(new [] { seriesCode })
                                                                      .Concat(BitConverter.GetBytes(organizationalID))
                                                                      .Concat(new [] { associationCategory })
                                                                      .ToArray();
        }

        protected static byte[] GSA75(UInt16 agencyCode, UInt16 systemSiteCode, UInt32 credentialNumber, UInt32 expirationDate)
        {
            return FASCN(agencyCode, systemSiteCode, credentialNumber).Concat(BitConverter.GetBytes(expirationDate)).ToArray();
        }

        protected static byte[] BCD(IEnumerable<byte> bytes)
        {
            var r = new BitArray(4*bytes.Count());//4 bits from each byte

            for (int i = 0; i < bytes.Count(); i++)
                for (int j = 0; j < 4; j++)
                    r[4*i + j] = 0 != (bytes.ElementAt(i) & (byte)(1 << j));

            return BitArrayToByteArray(r);
        }

        protected static byte[] FASCN_BCD(UInt32 agencyCode, UInt32 systemSiteCode, IEnumerable<byte> credentialNumber)
        {
            if (null == credentialNumber)
                throw new ArgumentNullException("credentialNumber");

            if (6 != credentialNumber.Count())
                throw new ArgumentException("credentialNumber: array should be of size 6");

            return BCD(BitConverter.GetBytes(agencyCode)
                                   .Concat(BitConverter.GetBytes(systemSiteCode))
                                   .Concat(credentialNumber));
        }

        protected static byte[] FASCNLarge_BCD(UInt32 agencyCode, UInt32 systemSiteCode, IEnumerable<byte> credentialNumber,
                                               byte seriesCode, byte credentialCode,
                                               IEnumerable<byte> personalID, 
                                               byte organizationalCategory, UInt32 organizationalID,
                                               byte associationCategory)
        {
            if (null == credentialNumber)
                throw new ArgumentNullException("credentialNumber");

            if (6 != credentialNumber.Count())
                throw new ArgumentException("credentialNumber: array should be of size 6");
            
            if (null == personalID)
                throw new ArgumentNullException("personalID");

            if (10 != personalID.Count())
                throw new ArgumentException("personalID: array should be of size 10");

            return BCD(BitConverter.GetBytes(agencyCode)
                                   .Concat(BitConverter.GetBytes(systemSiteCode))
                                   .Concat(credentialNumber)
                                   .Concat(new [] { seriesCode })
                                   .Concat(new [] { credentialCode })
                                   .Concat(personalID)
                                   .Concat(new [] { seriesCode })
                                   .Concat(BitConverter.GetBytes(organizationalID))
                                   .Concat(new [] { associationCategory }));
        }

        protected static byte[] ABA_TRACK2_BCD(IEnumerable<byte> primaryAccount,
                                               UInt32 expirationDate,
                                               UInt32 digitServiceCode,
                                               UInt32 discretionaryData)
        {
            if (null == primaryAccount)
                throw new ArgumentNullException("primaryAccount");

            if (19 != primaryAccount.Count())
                throw new ArgumentException("primaryAccount: array should be of size 19");


            var valueBytes = primaryAccount.Concat(new byte[]{ 0, 0, 0, 0 })
                                           .Concat(BitConverter.GetBytes(digitServiceCode).Take(3))
                                           .Concat(BitConverter.GetBytes(discretionaryData))
                                           .ToArray();

            var v = valueBytes[18];
            valueBytes[18] = valueBytes[19];
            valueBytes[19] = v;

            v = valueBytes[22];
            valueBytes[22] = valueBytes[23];
            valueBytes[23] = v;

            //Sample: date = July 2015 = 07.15
            //expirationDate(LS -> MS) = 0x05, 0x01, 0x07, 0x00
            //The date willbe written in hex-binary also as ...0715...
            var expirationDateBytes = BitConverter.GetBytes(expirationDate);
            valueBytes[18] = expirationDateBytes[3];
            valueBytes[20] = expirationDateBytes[1];
            valueBytes[21] = expirationDateBytes[2];
            valueBytes[23] = expirationDateBytes[0];

            return BCD(valueBytes);
        }

        protected static byte[] CHUID(UInt16 fascnAgencyCode, UInt16 fascnSystemSiteCode, UInt32 fascnCredentialNumber,
                                      UInt32 chuidAgencyCode, UInt32 chuidOrganizationID, IEnumerable<byte> chuidDUNSNumber,
                                      IEnumerable<byte> guid, UInt32 expirationDate)
        {
            return FASCN(fascnAgencyCode, fascnSystemSiteCode, fascnCredentialNumber).Concat(BitConverter.GetBytes(chuidAgencyCode))
                                                                                     .Concat(BitConverter.GetBytes(chuidOrganizationID))
                                                                                     .Concat(chuidDUNSNumber)
                                                                                     .Concat(GUID(guid))
                                                                                     .Concat(BitConverter.GetBytes(expirationDate))
                                                                                     .ToArray();
        }

        protected static byte[] buildLength(int length)
        {
            if (length <= 127)
                return new [] { (byte)length };


            if (length <= 255)
                return new [] { (byte)0x81, (byte)length, (byte)(length >> 8) };

            if (length <= 65535)
                return new [] { (byte)0x82, (byte)(length >> 8), (byte)length };

            throw new ArgumentException(string.Format("length: should be less or equal to 65535"));
        }

        protected static byte[] buildTLV(byte tag, IEnumerable<byte> value)
        {
            var r = new MemoryStream();
            using (var writer = new BinaryWriter(r))
            {
                writer.Write(tag);
                writer.Write(buildLength(value.Count()));
                writer.Write(value.ToArray());
            }

            return r.ToArray();
        }

        protected static byte[] CHUIDFull(UInt16 fascnAgencyCode, UInt16 fascnSystemSiteCode, UInt32 fascnCredentialNumber,
                                          IEnumerable<byte> guid, byte[] expirationDate)
        {
            if (null == expirationDate)
                throw new ArgumentNullException("expirationDate");

            if (8 != expirationDate.Count())
                throw new ArgumentException("expirationDate: array should be of size 8");

            ServiceExtensions.CheckCryptoLibary();

            var value = buildTLV(0x30, FASCN(fascnAgencyCode, fascnSystemSiteCode, fascnCredentialNumber)).Concat(buildTLV(0x34, GUID(guid)))
                                                                                                          .Concat(buildTLV(0x35, expirationDate));

            return buildTLV(0xEE, value.Concat(buildTLV(0x3E, Crypto.CryptoUtils.SignCHUIDByRandomKeyPair(value))));
        }

        protected static UInt32 ExpirationDate()
        {
            var afterMonth = DateTime.UtcNow.AddMonths(1);
            Int32 expirationDate = ((afterMonth.Year - 1900) & 0xff) | ((afterMonth.Month & 0xff) << 8) |
                                   ((afterMonth.Day & 0xff) << 16) | (0xff << 24);

            return (UInt32)expirationDate;
        }

        protected static byte[] ExpirationDateYYYYMMDD()
        {
            var afterMonth = DateTime.UtcNow.AddMonths(1);
            var expirationDate = new [] { (byte)((afterMonth.Year/1000) % 10), (byte)((afterMonth.Year/100) % 10), (byte)((afterMonth.Year/10) % 10), (byte)(afterMonth.Year % 10),
                                          (byte)((afterMonth.Month/10) % 10), (byte)(afterMonth.Month % 10),
                                          (byte)((afterMonth.Day/10) % 10), (byte)(afterMonth.Day % 10) };

            return expirationDate;
        }

        public static byte[] Create(string format, string salt)
        {
            var value = Encoding.UTF8.GetBytes((salt ?? "TestValue").GetHashCode().ToString());

            var binary = new byte[] {0, 0, 0, 0, 0, 0, 0, 0};
            
            var bytes = value;
            for (int i = 0; i < binary.Count() && i < bytes.Count(); ++i)
                binary[i] = bytes[i];

            switch (format)
            {
                case "WIEGAND26":
                    return Wiegand26(binary.First(), BitConverter.ToUInt16(binary, 1));
                case "WIEGAND37":
                    return Wiegand37WithoutFacilityCode(binary.Take(5));
                case "WIEGAND37_FACILITY":
                    return Wiegand37WithFacilityCode(BitConverter.ToUInt16(binary, 0), binary.Skip(2).Take(3).ToArray());
                case "FACILITY16_CARD32":
                    return Wiegand50WithFacilityCode(BitConverter.ToUInt16(binary, 0), binary.Skip(2).Take(4).ToArray());
                case "FACILITY32_CARD32":
                    return Wiegand66WithFacilityCode(BitConverter.ToUInt32(binary, 0), binary.Skip(4).Take(4).ToArray());
                case "FASC_N":
                    return FASCN(0x0102, 0x0304, BitConverter.ToUInt32(value.Take(4).ToArray(), 0));
                case "FASC_N_BCD":
                    return FASCN_BCD(0x01020304, 0x05060708, value.Take(6));
                case "FASC_N_LARGE":
                    return FASCNLarge(0x0102, 0x0304, BitConverter.ToUInt32(value.Take(4).ToArray(), 0), 0x01, 0x01, new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 }, 0x01, 0x0102, 0x01);
                case "FASC_N_LARGE_BCD":
                    return FASCNLarge_BCD(0x01020304, 0x05060708, value.Take(6), 0x01, 0x01, new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x01, 0x02, 0x03, 0x04, 0x05 }, 0x01, 0x01020304, 0x01);
                case "GSA75":
                    return GSA75(0x0102, 0x0304, BitConverter.ToUInt32(value.Take(4).ToArray(), 0), ExpirationDate());
                case "GUID":
                    return GUID(value);
                case "CHUID":
                    {
                        return CHUID(0x0102,
                                     0x0304,
                                     BitConverter.ToUInt32(value.Take(4).ToArray(), 0),
                                     0x01020304,
                                     0x01020304,
                                     new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09},
                                     value,
                                     ExpirationDate());
                    }
                case "CHUID_FULL":
                        return CHUIDFull(0x0102,
                                         0x0304,
                                         BitConverter.ToUInt32(value.Take(4).ToArray(), 0),
                                         value,
                                         ExpirationDateYYYYMMDD());
                //case "CBEFF_A":
                //case "CBEFF_B":
                //case "CBEFF_C":
                case "USER_PASSWORD":
                    return UsernamePassword(0x00, value, 0x00, value);
                case "SIMPLE_NUMBER16":
                    return SimpleNumber(16, binary);
                case "SIMPLE_NUMBER32":
                    return SimpleNumber(32, binary);
                case "SIMPLE_NUMBER56":
                    return SimpleNumber(56, binary);
                case "SIMPLE_ALPHA_NUMERIC":
                    return SimpleAlphaNumeric(0x00, value);
                case "ABA_TRACK2":
                    {
                        var afterMonth = DateTime.UtcNow.AddMonths(1);
                        //Int32 expirationDate = ((afterMonth.Month % 10) & 0xff) | (((afterMonth.Month/10 % 10) & 0xff) << 8) 
                        //                     | (((afterMonth.Year % 10) & 0xff) << 16) | (((afterMonth.Year/10 % 10) & 0xff) << 24); 
                        Int32 expirationDate = ((afterMonth.Year % 10) & 0xff) | (((afterMonth.Year/10 % 10) & 0xff) << 8) 
                                             | (((afterMonth.Month % 10) & 0xff) << 16) | (((afterMonth.Month/10 % 10) & 0xff) << 24); 
                        return ABA_TRACK2_BCD(value.Take(4).Concat(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x01, 0x02, 0x03, 0x04, 0x05, 0x01, 0x02, 0x03, 0x04, 0x05 }),
                                              (UInt32)expirationDate,
                                              0x00010203,
                                              0x01020304);
                    }
            }
            throw new ArgumentException("format: unsupported format");
        }
    }
}
