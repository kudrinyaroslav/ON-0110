using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using DUT.WithLogic.Base;
using System.Net.NetworkInformation;
using DUT.WithLogic.Engine;

namespace DUT.WithLogic.Services.Media2
{
    public class ONVIFMedia2Configuration
    {
        #region Members

        #region VideoSources

        List<Proxy.VideoSourceConfiguration> m_VideoSourceConfigurationList;

        #endregion //VideoSources

        #region OSD

        List<Proxy.OSDConfiguration> m_OSDConfigurationList;
        List<VideoSourceConfiguration_OSDConfigurationOptions> m_OSDOptionsList;

        #endregion //OSD

        #endregion //Members

        #region Properties

        public List<VideoSourceConfiguration_OSDConfigurationOptions> OSDOptionsList
        {
            get { return m_OSDOptionsList; }
            set { m_OSDOptionsList = value; }
        }

        public List<Proxy.VideoSourceConfiguration> VideoSourceConfigurationList
        {
            get { return m_VideoSourceConfigurationList; }
            set { m_VideoSourceConfigurationList = value; }
        }

        public List<Proxy.OSDConfiguration> OSDConfigurationList
        {
            get { return m_OSDConfigurationList; }
            set { m_OSDConfigurationList = value; }
        }

        #endregion //Properties

        private static ONVIFMedia2Configuration Load(string configurationPath)
        {
            using (XmlReader reader = XmlReader.Create(Engine.ONVIFServiceList.FullUri(configurationPath)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ONVIFMedia2Configuration));
                return (ONVIFMedia2Configuration)serializer.Deserialize(reader);
            }

        }

        public static void Serialize()
        {
            ONVIFMedia2Configuration tmp = new ONVIFMedia2Configuration();

            //VideoSourceConfigurationList
            tmp.VideoSourceConfigurationList = new List<Proxy.VideoSourceConfiguration>();
            Proxy.VideoSourceConfiguration vsc;
            vsc = new Proxy.VideoSourceConfiguration();
            vsc.UseCount = 9;
            vsc.Bounds = new Proxy.IntRectangle();
            vsc.Bounds.height = 900;
            vsc.Bounds.width = 600;
            vsc.Bounds.x = 0;
            vsc.Bounds.y = 0;
            vsc.Name = "VSC Name 1";
            vsc.SourceToken = "VS1";
            vsc.token = "VSC1";
            vsc.Extension = new Proxy.VideoSourceConfigurationExtension();
            vsc.Extension.Rotate = new Proxy.Rotate();
            vsc.Extension.Rotate.Mode = Proxy.RotateMode.ON;
            vsc.Extension.Rotate.DegreeSpecified = true;
            vsc.Extension.Rotate.Degree = 10;
            vsc.Extension.Extension = new Proxy.VideoSourceConfigurationExtension2();
            vsc.Extension.Extension.LensDescription = new Proxy.LensDescription[1];
            Proxy.LensDescription ld = new Proxy.LensDescription();
            ld.FocalLengthSpecified = true;
            ld.FocalLength = 1;
            ld.Offset = new Proxy.LensOffset();
            ld.Offset.xSpecified = true;
            ld.Offset.x = 1;
            ld.Offset.ySpecified = true;
            ld.Offset.y = 1;
            ld.Projection = null;
            ld.XFactor = 1;

            vsc.Extension.Extension.LensDescription[0] = ld;
            tmp.VideoSourceConfigurationList.Add(vsc);

            //OSDConfigurationList
            tmp.OSDConfigurationList = new List<Proxy.OSDConfiguration>();
            Proxy.OSDConfiguration OSDConf;
            OSDConf = new Proxy.OSDConfiguration();
            OSDConf.Image = new Proxy.OSDImgConfiguration();
            OSDConf.Image.ImgPath = "path";
            OSDConf.Position = new Proxy.OSDPosConfiguration();
            OSDConf.Position.Pos = new Proxy.Vector();
            OSDConf.Position.Pos.xSpecified = true;
            OSDConf.Position.Pos.x = 1;
            OSDConf.Position.Pos.ySpecified = true;
            OSDConf.Position.Pos.y = 1;
            OSDConf.Position.Type = "type";
            OSDConf.TextString = new Proxy.OSDTextConfiguration();
            OSDConf.TextString.BackgroundColor = new Proxy.OSDColor();
            OSDConf.TextString.BackgroundColor.Color = new Proxy.Color();
            OSDConf.TextString.BackgroundColor.Color.Colorspace = "Colorspace";
            OSDConf.TextString.BackgroundColor.Color.X = 1;
            OSDConf.TextString.BackgroundColor.Color.Y = 1;
            OSDConf.TextString.BackgroundColor.Color.Z = 1;
            OSDConf.TextString.BackgroundColor.TransparentSpecified = true;
            OSDConf.TextString.BackgroundColor.Transparent = 20;
            OSDConf.TextString.DateFormat = "xxxx";
            OSDConf.TextString.FontColor = OSDConf.TextString.BackgroundColor;
            OSDConf.TextString.FontSizeSpecified = true;
            OSDConf.TextString.FontSize = 12;
            OSDConf.TextString.PlainText = "test";
            OSDConf.TextString.TimeFormat = "ffff";
            OSDConf.TextString.Type = "sdfdsf";
            OSDConf.token = "OSDConf1";
            OSDConf.Type = Proxy.OSDType.Text;
            OSDConf.VideoSourceConfigurationToken = new Proxy.OSDReference();
            OSDConf.VideoSourceConfigurationToken.Value = "VSC1";

            tmp.OSDConfigurationList.Add(OSDConf);

            //OSDOptionsList
            tmp.OSDOptionsList = new List<VideoSourceConfiguration_OSDConfigurationOptions>();
            VideoSourceConfiguration_OSDConfigurationOptions VSC_OSDOp = new VideoSourceConfiguration_OSDConfigurationOptions();
            VSC_OSDOp.VideoSourceConfigurationToken1 = "VSC1";
            VSC_OSDOp.OSDConfigurationOptions = new Proxy.OSDConfigurationOptions();
            VSC_OSDOp.OSDConfigurationOptions.ImageOption = new Proxy.OSDImgOptions();
            VSC_OSDOp.OSDConfigurationOptions.ImageOption.ImagePath = new string[] { "eee", "eeeer" };
            VSC_OSDOp.OSDConfigurationOptions.MaximumNumberOfOSDs = new Proxy.MaximumNumberOfOSDs();
            VSC_OSDOp.OSDConfigurationOptions.MaximumNumberOfOSDs.Date = 1;
            VSC_OSDOp.OSDConfigurationOptions.MaximumNumberOfOSDs.DateSpecified = true;
            VSC_OSDOp.OSDConfigurationOptions.MaximumNumberOfOSDs.DateAndTimeSpecified = true;
            VSC_OSDOp.OSDConfigurationOptions.MaximumNumberOfOSDs.DateAndTime = 2;
            VSC_OSDOp.OSDConfigurationOptions.MaximumNumberOfOSDs.ImageSpecified = true;
            VSC_OSDOp.OSDConfigurationOptions.MaximumNumberOfOSDs.Image = 3;
            VSC_OSDOp.OSDConfigurationOptions.MaximumNumberOfOSDs.PlainTextSpecified = true;
            VSC_OSDOp.OSDConfigurationOptions.MaximumNumberOfOSDs.PlainText = 4;
            VSC_OSDOp.OSDConfigurationOptions.MaximumNumberOfOSDs.TimeSpecified = true;
            VSC_OSDOp.OSDConfigurationOptions.MaximumNumberOfOSDs.Time = 1;
            VSC_OSDOp.OSDConfigurationOptions.MaximumNumberOfOSDs.Total = 5;
            VSC_OSDOp.OSDConfigurationOptions.PositionOption = new string[] { "eee", "eeeer" };
            VSC_OSDOp.OSDConfigurationOptions.TextOption = new Proxy.OSDTextOptions();
            VSC_OSDOp.OSDConfigurationOptions.TextOption.BackgroundColor = new Proxy.OSDColorOptions();
            VSC_OSDOp.OSDConfigurationOptions.TextOption.BackgroundColor.Color = new Proxy.ColorOptions();
            //VSC_OSDOp.OSDConfigurationOptions.TextOption.BackgroundColor.Color.Items = new object[] { "eee", "eeeer" };
            VSC_OSDOp.OSDConfigurationOptions.TextOption.BackgroundColor.Transparent = new Proxy.IntRange();
            VSC_OSDOp.OSDConfigurationOptions.TextOption.BackgroundColor.Transparent.Min = 0;
            VSC_OSDOp.OSDConfigurationOptions.TextOption.BackgroundColor.Transparent.Max = 10;
            VSC_OSDOp.OSDConfigurationOptions.TextOption.DateFormat = new string[] { "eee", "eeeer" };
            VSC_OSDOp.OSDConfigurationOptions.TextOption.FontColor = VSC_OSDOp.OSDConfigurationOptions.TextOption.BackgroundColor;
            VSC_OSDOp.OSDConfigurationOptions.TextOption.FontSizeRange = VSC_OSDOp.OSDConfigurationOptions.TextOption.BackgroundColor.Transparent;
            VSC_OSDOp.OSDConfigurationOptions.TextOption.TimeFormat = new string[] { "eee", "eeeer" };
            VSC_OSDOp.OSDConfigurationOptions.TextOption.Type = new string[] { "eee", "eeeer" };
            VSC_OSDOp.OSDConfigurationOptions.Type = new Proxy.OSDType[] { Proxy.OSDType.Text, Proxy.OSDType.Image, Proxy.OSDType.Extended };

            tmp.OSDOptionsList.Add(VSC_OSDOp);

            using (XmlWriter writer = XmlWriter.Create(@"D:\3.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ONVIFMedia2Configuration));
                serializer.Serialize(writer, tmp);
            }
        }

        public static ONVIFMedia2Configuration Load()
        {
            return Load(Base.AppPaths.PATH_MEDAI2CONFIGURATION);
        }



        public static ONVIFMedia2Configuration HardReset()
        {
            return Load(Base.AppPaths.PATH_MEDAI2CONFIGURATION_FACTORYDEFAULTS);
        }

        public static void OSDParametersCheck(Proxy.OSDConfiguration OSD, Proxy.OSDConfigurationOptions OSDOpt, List<Proxy.OSDConfiguration> OSDList)
        {

            #region CountChecks

            if (OSDList.Count >= OSDOpt.MaximumNumberOfOSDs.Total)
            {
                throw ONVIFFault.GetMedia2Exception_Action_MaxOSDs(OSD.VideoSourceConfigurationToken.Value);
            }

            if (OSDOpt.MaximumNumberOfOSDs.ImageSpecified &&
                OSDList.Count(C => C.Image != null) >= OSDOpt.MaximumNumberOfOSDs.Image)
            {
                throw ONVIFFault.GetMedia2Exception_Action_MaxOSDs(OSD.VideoSourceConfigurationToken.Value);
            }

            if (OSDOpt.MaximumNumberOfOSDs.PlainTextSpecified &&
                OSDList.Count(C => (C.TextString != null) && (C.TextString.PlainText != null)) >= OSDOpt.MaximumNumberOfOSDs.PlainText)
            {
                throw ONVIFFault.GetMedia2Exception_Action_MaxOSDs(OSD.VideoSourceConfigurationToken.Value);
            }

            if (OSDOpt.MaximumNumberOfOSDs.DateSpecified &&
                OSDList.Count(C => (C.TextString != null) && (C.TextString.Type == "Date")) >= OSDOpt.MaximumNumberOfOSDs.Date)
            {
                throw ONVIFFault.GetMedia2Exception_Action_MaxOSDs(OSD.VideoSourceConfigurationToken.Value);
            }

            if (OSDOpt.MaximumNumberOfOSDs.DateAndTimeSpecified &&
                OSDList.Count(C => (C.TextString != null) && (C.TextString.Type == "DateAndTime")) >= OSDOpt.MaximumNumberOfOSDs.DateAndTime)
            {
                throw ONVIFFault.GetMedia2Exception_Action_MaxOSDs(OSD.VideoSourceConfigurationToken.Value);
            }

            if (OSDOpt.MaximumNumberOfOSDs.PlainTextSpecified &&
                OSDList.Count(C => (C.TextString != null) && (C.TextString.Type == "Plain")) >= OSDOpt.MaximumNumberOfOSDs.PlainText)
            {
                throw ONVIFFault.GetMedia2Exception_Action_MaxOSDs(OSD.VideoSourceConfigurationToken.Value);
            }

            if (OSDOpt.MaximumNumberOfOSDs.TimeSpecified &&
                OSDList.Count(C => (C.TextString != null) && (C.TextString.Type == "Time")) >= OSDOpt.MaximumNumberOfOSDs.Time)
            {
                throw ONVIFFault.GetMedia2Exception_Action_MaxOSDs(OSD.VideoSourceConfigurationToken.Value);
            }

            #endregion //CountChecks

            //OSD.Type
            if (!OSDOpt.Type.Any(C => C == OSD.Type))
            {
                throw ONVIFFault.GetGeneralException_InvalidArgVal("OSD Type is not supported.");
            }

            //OSD.Position.Type
            if (!OSDOpt.PositionOption.Any(C => C == OSD.Position.Type))
            {
                throw ONVIFFault.GetGeneralException_InvalidArgVal("OSD Position Type is not supported.");
            }

            if (OSD.Image != null)
            {
                //TODO: validation for Image
            }

            if (OSD.TextString != null)
            {
                if (OSDOpt.TextOption == null)
                {
                    throw ONVIFFault.GetGeneralException_InvalidArgVal("OSD Text is not supported.");
                }

                //OSD.TextString.Type
                if (!OSDOpt.TextOption.Type.Any(C => C == OSD.TextString.Type))
                {
                    throw ONVIFFault.GetGeneralException_InvalidArgVal("OSD Text Type is not supported.");
                }

                //OSD.TextString.DateFormat
                if ((OSD.TextString.Type == "Date") || (OSD.TextString.Type == "DateAndTime"))
                {
                    if (OSD.TextString.DateFormat == null)
                    {
                        OSD.TextString.DateFormat = OSDOpt.TextOption.DateFormat[0];
                    }
                    else
                    {
                        if (!OSDOpt.TextOption.DateFormat.Any(C => C == OSD.TextString.DateFormat))
                        {
                            throw ONVIFFault.GetGeneralException_InvalidArgVal("OSD Date Format is not supported.");
                        }
                    }
                }

                //OSD.TextString.TimeFormat
                if ((OSD.TextString.Type == "Time") || (OSD.TextString.Type == "DateAndTime"))
                {
                    if (OSD.TextString.TimeFormat == null)
                    {
                        OSD.TextString.TimeFormat = OSDOpt.TextOption.TimeFormat[0];
                    }
                    else
                    {
                        if (!OSDOpt.TextOption.TimeFormat.Any(C => C == OSD.TextString.TimeFormat))
                        {
                            throw ONVIFFault.GetGeneralException_InvalidArgVal("OSD Time Format is not supported.");
                        }
                    }
                }

                //OSD.TextString.FontSize
                if (OSD.TextString.FontSizeSpecified)
                {
                    if (OSDOpt.TextOption.FontSizeRange == null)
                    {
                        throw ONVIFFault.GetGeneralException_InvalidArgVal("OSD FontSize is not supported.");
                    }
                    else
                    {
                        if ((OSD.TextString.FontSize > OSDOpt.TextOption.FontSizeRange.Max) ||
                            (OSD.TextString.FontSize < OSDOpt.TextOption.FontSizeRange.Min))
                        {
                            throw ONVIFFault.GetGeneralException_InvalidArgVal("OSD FontSize is out of supported range.");
                        }
                    }
                }
                else
                {
                    if (OSDOpt.TextOption.FontSizeRange != null)
                    {
                        OSD.TextString.FontSizeSpecified = true;
                        OSD.TextString.FontSize = OSDOpt.TextOption.FontSizeRange.Min;
                    }
                }

                //OSD.TextString.FontColor
                OSDColorCheck(OSD.TextString.FontColor, OSDOpt.TextOption.FontColor, "FontColor");

                //OSD.TextString.BackgroundColor
                OSDColorCheck(OSD.TextString.BackgroundColor, OSDOpt.TextOption.BackgroundColor, "BackgroundColor");

            }
        }






        public static void OSDColorCheck(Proxy.OSDColor oSDColor, Proxy.OSDColorOptions oSDColorOptions, string ParameterName)
        {
            if (oSDColor != null)
            {
                if (oSDColorOptions == null)
                {
                    throw ONVIFFault.GetGeneralException_InvalidArgVal(String.Format("OSD {0} is not supported.", ParameterName));
                }
                else
                {
                    if (oSDColor.Color != null)
                    {
                        if ((oSDColorOptions.Color == null) ||
                            (oSDColorOptions.Color.Items == null))
                        {
                            throw ONVIFFault.GetGeneralException_InvalidArgVal(String.Format("OSD {0}.Color is not supported.", ParameterName));
                        }
                        else
                        {
                            //TODO: OSD.TextString.FontColor.Color range check
                        }
                    }
                    else
                    {
                        if (oSDColorOptions.Color != null)
                        {
                            //TODOD: color
                        }
                    }

                    if (oSDColor.TransparentSpecified)
                    {
                        if ((oSDColorOptions.Transparent == null))
                        {
                            throw ONVIFFault.GetGeneralException_InvalidArgVal(String.Format("OSD {0}.Transparent is not supported.", ParameterName));
                        }
                        else
                        {
                            if ((oSDColor.Transparent > oSDColorOptions.Transparent.Max) ||
                                (oSDColor.Transparent < oSDColorOptions.Transparent.Min))
                            {
                                throw ONVIFFault.GetGeneralException_InvalidArgVal(String.Format("OSD {0}.Transparent is out of supported range.", ParameterName));
                            }
                        }
                    }
                    else
                    {
                        if (oSDColorOptions.Transparent != null)
                        {
                            oSDColor.TransparentSpecified = true;
                            oSDColor.Transparent = oSDColorOptions.Transparent.Min;
                        }
                    }
                }
            }
            else
            {
                if (oSDColorOptions != null)
                {
                    oSDColor = new Proxy.OSDColor();
                    if (oSDColorOptions.Transparent != null)
                    {
                        oSDColor.TransparentSpecified = true;
                        oSDColor.Transparent = oSDColorOptions.Transparent.Min;
                    }

                    if (oSDColorOptions.Color != null)
                    {
                        //TODOD: color
                    }
                }
            }
        }

    }

    public class VideoSourceConfiguration_OSDConfigurationOptions
    {
        private string VideoSourceConfigurationToken;

        public string VideoSourceConfigurationToken1
        {
            get { return VideoSourceConfigurationToken; }
            set { VideoSourceConfigurationToken = value; }
        }
        private Proxy.OSDConfigurationOptions m_OSDConfigurationOptions;

        public Proxy.OSDConfigurationOptions OSDConfigurationOptions
        {
            get { return m_OSDConfigurationOptions; }
            set { m_OSDConfigurationOptions = value; }
        }
    }
}