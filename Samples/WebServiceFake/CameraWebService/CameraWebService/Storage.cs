using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CameraWebService
{
    public class Storage
    {
        private static Storage _instance; 
        public static Storage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Storage();
                }
                return _instance;
            }
        }

        private Media.VideoSource[] _sources;
        public Media.VideoSource[] Sources
        {
            get
            {
                if (_sources == null)
                {
                    List<Media.VideoSource> lst= new List<Media.VideoSource>();

                    for (int i = 1; i < 5; i++)
                    {

                        Media.VideoSource source = new Media.VideoSource();

                        source.token = "source" + i;
                        source.Resolution = new Media.VideoResolution();
                        source.Resolution.Height = 480;
                        source.Resolution.Width = 640;

                        source.Imaging = new Media.ImagingSettings();
                        source.Imaging.BacklightCompensation = new Media.BacklightCompensation();
                        source.Imaging.BacklightCompensation.Mode = Media.BacklightCompensationMode.OFF;

                        lst.Add(source);

                    }
                    _sources = lst.ToArray();
                }

                return _sources;
            }
        }

        private Dictionary<string, Imaging.MoveOptions20> _moveOptions;

        public Dictionary<string, Imaging.MoveOptions20> MoveOptions
        {
            get
            {
                if (_moveOptions == null)
                {
                    _moveOptions = new Dictionary<string, Imaging.MoveOptions20>();
                    
                    foreach (Media.VideoSource source in Sources)
                    {
                        Imaging.MoveOptions20 opt = new Imaging.MoveOptions20();
                        
                        System.Random random = new Random();
                        int i = random.Next(10);

                        if (i % 2 == 1)
                        {
                            opt.Absolute = new Imaging.AbsoluteFocusOptions();
                            opt.Absolute.Position = new Imaging.FloatRange();
                            opt.Absolute.Position.Min = 10;
                            opt.Absolute.Position.Max = 20;

                            opt.Absolute.Speed = new Imaging.FloatRange();
                            opt.Absolute.Speed.Max = 30;
                            opt.Absolute.Speed.Min = 20;
                        }

                        if (i % 2 == 0)
                        {
                            opt.Continuous = new Imaging.ContinuousFocusOptions();
                            opt.Continuous.Speed = new Imaging.FloatRange();
                            opt.Continuous.Speed.Min = 5;
                            opt.Continuous.Speed.Max = 10;
                        }

                        opt.Relative = new Imaging.RelativeFocusOptions20();
                        opt.Relative.Distance = new Imaging.FloatRange();
                        opt.Relative.Distance.Min = -10;
                        opt.Relative.Distance.Max = 10;

                        opt.Relative.Speed = new Imaging.FloatRange();
                        opt.Relative.Speed.Min = 10;
                        opt.Relative.Speed.Max = 15;

                        _moveOptions.Add(source.token, opt);

                    }
                }

                return _moveOptions;
            }
        }


        private List<RelayOutput> _relayOutputs;
        public List<RelayOutput> RelayOutputs
        {
            get
            {
                if (_relayOutputs == null)
                {
                    _relayOutputs = new List<RelayOutput>();

                    RelayOutput ro1 = new RelayOutput();
                    ro1.token = "output1";
                    ro1.Properties = new RelayOutputSettings();
                    ro1.Properties.DelayTime = "PT10S";
                    ro1.Properties.IdleState = RelayIdleState.open;
                    ro1.Properties.Mode = RelayMode.Bistable;
                    _relayOutputs.Add(ro1);


                    RelayOutput ro2 = new RelayOutput();
                    ro2.token = "output2";
                    ro2.Properties = new RelayOutputSettings();
                    ro2.Properties.DelayTime = "PT40S";
                    ro2.Properties.IdleState = RelayIdleState.open;
                    ro2.Properties.Mode = RelayMode.Monostable ;
                    _relayOutputs.Add(ro2);


                    RelayOutput ro3 = new RelayOutput();
                    ro3.token = "output3";
                    ro3.Properties = new RelayOutputSettings();
                    ro3.Properties.DelayTime = "PT25S";
                    ro3.Properties.IdleState = RelayIdleState.closed;
                    ro3.Properties.Mode = RelayMode.Monostable;
                    _relayOutputs.Add(ro3);



                }
                return _relayOutputs;
            }

        }

        private int _testRunCounter = 0;
        public int TestRunCounter
        {
            get { return _testRunCounter; }
            set { _testRunCounter = value;}
        }

        public void IncTestRunCounter()
        {
            _testRunCounter++;
        }

    }

    public static class Helper
    {
        public static bool NotIn(this float value, Imaging.FloatRange range)
        {
            return (value > range.Max || value < range.Min);

        }
    }

}
