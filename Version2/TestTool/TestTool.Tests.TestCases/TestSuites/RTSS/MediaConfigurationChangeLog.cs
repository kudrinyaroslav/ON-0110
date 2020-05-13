using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Proxies.Onvif;

namespace TestTool.Tests.TestCases.TestSuites
{
    public class MediaConfigurationChangeLog
    {
        public int LastProfileNumberUsed { get; set; }

        List<Profile> _deletedProfiles = new List<Profile>();
        public List<Profile> DeletedProfiles
        {
            get { return _deletedProfiles; }
        }

        List<Profile> _createdProfiles = new List<Profile>();
        public List<Profile> CreatedProfiles
        {
            get { return _createdProfiles; }
        }

        List<Profile> _modifiedProfiles = new List<Profile>();
        public List<Profile> ModifiedProfiles
        {
            get { return _modifiedProfiles; }
        }

        List<VideoEncoderConfiguration> _modifiedVideoEncoderConfigurations =
            new List<VideoEncoderConfiguration>();
        public List<VideoEncoderConfiguration> ModifiedVideoEncoderConfigurations
        {
            get { return _modifiedVideoEncoderConfigurations; }
        }

        List<MetadataConfiguration> _modifiedMetadataConfigurations =
            new List<MetadataConfiguration>();
        public List<MetadataConfiguration> ModifiedMetadataConfigurations
        {
            get { return _modifiedMetadataConfigurations; }
        }

        List<AudioEncoderConfiguration> _modifiedAudioEncoderConfigurations =
            new List<AudioEncoderConfiguration>();
        public List<AudioEncoderConfiguration> ModifiedAudioEncoderConfigurations
        {
            get { return _modifiedAudioEncoderConfigurations; }
        }

        List<AudioOutputConfiguration> _modifiedAudioOutputConfigurations =
            new List<AudioOutputConfiguration>();
        public List<AudioOutputConfiguration> ModifiedAudioOutputConfigurations
        {
          get { return _modifiedAudioOutputConfigurations; }
        }
        List<AudioDecoderConfiguration> _modifiedAudioDecoderConfigurations =
            new List<AudioDecoderConfiguration>();
        public List<AudioDecoderConfiguration> ModifiedAudioDecoderConfigurations
        {
          get { return _modifiedAudioDecoderConfigurations; }
        }

        public void TrackDeletedProfile(Profile profile)
        {
            Profile created = _createdProfiles.Where(P => P.token == profile.token).FirstOrDefault();
            if (created != null)
            {
                _createdProfiles.Remove(created);
            }
            else 
            {
                _deletedProfiles.Add(profile);
            }        
        }

        public void TrackModifiedProfile(Profile profile)
        {
            Profile created = _createdProfiles.Where(P => P.token == profile.token).FirstOrDefault();
            Profile modified = _modifiedProfiles.Where(P => P.token == profile.token).FirstOrDefault();
            if (created == null  && modified==null)
            {
                _modifiedProfiles.Add(profile);
            }
        }

        public void TrackModifiedConfiguration(AudioEncoderConfiguration config)
        {
            AudioEncoderConfiguration modified = _modifiedAudioEncoderConfigurations.Where(AEC => AEC.token == config.token).FirstOrDefault();
            if (modified == null)
            {
                _modifiedAudioEncoderConfigurations.Add(config);
            }
        }

        public void TrackModifiedConfiguration(VideoEncoderConfiguration config)
        {
            VideoEncoderConfiguration modified = _modifiedVideoEncoderConfigurations.Where(AEC => AEC.token == config.token).FirstOrDefault();
            if (modified == null)
            {
                _modifiedVideoEncoderConfigurations.Add(config);
            }
        }

        public void TrackModifiedConfiguration(MetadataConfiguration config)
        {
          MetadataConfiguration modified = _modifiedMetadataConfigurations.Where(AEC => AEC.token == config.token).FirstOrDefault();
          if (modified == null)
          {
            _modifiedMetadataConfigurations.Add(config);
          }
        }
        public void TrackModifiedConfiguration(AudioOutputConfiguration config)
        {
          AudioOutputConfiguration modified = _modifiedAudioOutputConfigurations.Where(AEC => AEC.token == config.token).FirstOrDefault();
          if (modified == null)
          {
            _modifiedAudioOutputConfigurations.Add(config);
          }
        }
        public void TrackModifiedConfiguration(AudioDecoderConfiguration config)
        {
          AudioDecoderConfiguration modified = _modifiedAudioDecoderConfigurations.Where(AEC => AEC.token == config.token).FirstOrDefault();
          if (modified == null)
          {
            _modifiedAudioDecoderConfigurations.Add(config);
          }
        }

    }

}
