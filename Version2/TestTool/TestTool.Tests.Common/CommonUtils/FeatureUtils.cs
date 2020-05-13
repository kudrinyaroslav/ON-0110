using System;
using System.Collections.Generic;
using System.Linq;
using TestTool.Tests.Definitions.Enums;

namespace TestTool.Tests.Common.CommonUtils
{

    public static class FeatureUtils
    {
        public static bool ContainsFeature(this IEnumerable<Feature> features, Feature feature)
        {
            bool local = false;

            switch (feature)
            {
                case Feature.PTZAbsoluteOrRelative:
                    local = features.Contains(Feature.PTZAbsolute) || features.Contains(Feature.PTZRelative);
                    break;
                case Feature.H264OrMPEG4:
                    local = features.Contains(Feature.H264) || features.Contains(Feature.MPEG4);
                    break;
                case Feature.PTZAbsoluteOrRelativePanTilt:
                    local = features.Contains(Feature.PTZAbsolutePanTilt) || features.Contains(Feature.PTZRelativePanTilt);
                    break;
                case Feature.PTZAbsoluteOrRelativeZoom:
                    local = features.Contains(Feature.PTZAbsoluteZoom) || features.Contains(Feature.PTZRelativeZoom);
                    break;
                case Feature.MediaOrReceiver:
                    local = features.Contains(Feature.MediaService) || features.Contains(Feature.ReceiverService);
                    break;
                case Feature.DynamicRecordingsOrDynamicTracks:
                    local = features.Contains(Feature.DynamicRecordings) || features.Contains(Feature.DynamicTracks);
                    break;
                case Feature.HttpSystemBackupOrHttpSystemLoggingOrHttpSupportInformation:
                    local = features.Contains(Feature.HttpSystemBackup) || features.Contains(Feature.HttpSystemLogging) || features.Contains(Feature.HttpSupportInformation);
                    break;
                case Feature.RSAKeyPairGenerationOrPKCS8RSAKeyPairUpload:
                    local = features.Contains(Feature.RSAKeyPairGeneration) || features.Contains(Feature.PKCS8RSAKeyPairUpload);
                    break;
                case Feature.SelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA:
                    local = features.Contains(Feature.SelfSignedCertificateCreationWithRSA) && features.Contains(Feature.PKCS10ExternalCertificationWithRSA);
                    break;
                case Feature.RSAKeyPairGenerationAndSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA:
                    local = features.Contains(Feature.RSAKeyPairGeneration) && features.Contains(Feature.SelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA);
                    break;
                case Feature.PKCS12CertificateWithRSAPrivateKeyUploadOrTLSServerSupport:
                    local = features.Contains(Feature.PKCS12CertificateWithRSAPrivateKeyUpload) || features.Contains(Feature.TLSServerSupport);
                    break;
                case Feature.PKCS12CertificateWithRSAPrivateKeyUploadOrRSAKeyPairGenerationAndSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA:
                    local = features.Contains(Feature.PKCS12CertificateWithRSAPrivateKeyUpload) || features.Contains(Feature.RSAKeyPairGenerationAndSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA);
                    break;
                case Feature.RSAKeyPairGenerationOrSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA:
                    local = features.Contains(Feature.RSAKeyPairGeneration) || features.ContainsFeature(Feature.SelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA);
                    break;
                case Feature.PKCS8RSAKeyPairUploadOrPKCS12CertificateWithRSAPrivateKeyUpload:
                    local = features.Contains(Feature.PKCS8RSAKeyPairUpload) || features.Contains(Feature.PKCS12CertificateWithRSAPrivateKeyUpload);
                    break;
                case Feature.RSAKeyPairGenerationOrPKCS8RSAKeyPairUploadOrSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA:
                    local = features.ContainsFeature(Feature.RSAKeyPairGenerationOrPKCS8RSAKeyPairUpload) || features.ContainsFeature(Feature.SelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA);
                    break;
                case Feature.RSAKeyPairGenerationOrPKCS8RSAKeyPairUploadOrSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSAOrPKCS12CertificateWithRSAPrivateKeyUpload:
                    local = features.ContainsFeature(Feature.RSAKeyPairGenerationOrPKCS8RSAKeyPairUpload) || features.ContainsFeature(Feature.SelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA) || features.ContainsFeature(Feature.PKCS12CertificateWithRSAPrivateKeyUpload);
                    break;
                case Feature.RSAKeyPairGenerationOrTLSServerSupport:
                    local = features.Contains(Feature.RSAKeyPairGeneration) || features.Contains(Feature.TLSServerSupport);
                    break;
                case Feature.RSAKeyPairGenerationOrPKCS12CertificateWithRSAPrivateKeyUpload:
                    local = features.Contains(Feature.RSAKeyPairGeneration) || features.Contains(Feature.PKCS12CertificateWithRSAPrivateKeyUpload);
                    break;
                case Feature.CredentialValidityOrCredentialAccessProfileValidity:
                    local = features.Contains(Feature.CredentialValidity) || features.Contains(Feature.CredentialAccessProfileValidity);
                    break;
                default:
                    local = features.Contains(feature);
                    break;
            }
            return local;
        }

        public static bool IsCompoundFeature(Feature feature)
        {
            switch (feature)
            {
                case Feature.PTZAbsoluteOrRelative:
                case Feature.H264OrMPEG4:
                case Feature.PTZAbsoluteOrRelativePanTilt:
                case Feature.PTZAbsoluteOrRelativeZoom:
                case Feature.MediaOrReceiver:
                case Feature.DynamicRecordingsOrDynamicTracks:
                case Feature.HttpSystemBackupOrHttpSystemLoggingOrHttpSupportInformation:
                case Feature.RSAKeyPairGenerationOrPKCS8RSAKeyPairUpload:
                case Feature.SelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA:
                case Feature.RSAKeyPairGenerationOrSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA:
                case Feature.PKCS8RSAKeyPairUploadOrPKCS12CertificateWithRSAPrivateKeyUpload:
                case Feature.RSAKeyPairGenerationOrPKCS8RSAKeyPairUploadOrSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA:
                case Feature.RSAKeyPairGenerationOrPKCS8RSAKeyPairUploadOrSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSAOrPKCS12CertificateWithRSAPrivateKeyUpload:
                case Feature.RSAKeyPairGenerationOrTLSServerSupport:
                case Feature.CredentialValidityOrCredentialAccessProfileValidity:
                case Feature.RSAKeyPairGenerationOrPKCS12CertificateWithRSAPrivateKeyUpload:
                case Feature.RSAKeyPairGenerationAndSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA:
                case Feature.PKCS12CertificateWithRSAPrivateKeyUploadOrRSAKeyPairGenerationAndSelfSignedCertificateCreationWithRSAAndPKCS10ExternalCertificationWithRSA:
                case Feature.PKCS12CertificateWithRSAPrivateKeyUploadOrTLSServerSupport:
                    return true;
            }

            return false;
        }

    }

}
