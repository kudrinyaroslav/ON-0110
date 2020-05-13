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
                    return true;
            }

            return false;
        }

    }

}
