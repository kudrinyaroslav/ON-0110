using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace DUT.CameraWebService
{
    public class CommonCompare
    {
        public static void StringCompare(string requestXPath, string TargetName, string TargetString, ref string logMessage, ref bool passed, XmlNode test)
        {
            if (TargetString != null)
            {
                if (test.SelectNodes(requestXPath).Count != 0)
                {
                    if ((test.SelectSingleNode(requestXPath).NodeType != XmlNodeType.Element)|| ((test.SelectSingleNode(requestXPath).Attributes["ignore"] == null) || 
                        (test.SelectSingleNode(requestXPath).Attributes["ignore"].InnerXml != "1")))
                    {
                        if (test.SelectSingleNode(requestXPath).InnerText != TargetString)
                        {
                            passed = false;
                            logMessage = logMessage + TargetName + " = " + test.SelectSingleNode(requestXPath).InnerText + " but found " + TargetString + ". ";
                        }
                    }
                    else
                    {
                        logMessage = logMessage + TargetName + " = " + TargetString + ". ";
                    }
                }
                else
                {
                    passed = false;
                    logMessage = logMessage + "Unexpected " + TargetName + ". ";
                }
            }
            else
            {
                if (test.SelectNodes(requestXPath).Count != 0)
                {
                    passed = false;
                    logMessage = logMessage + "No expected " + TargetName + ". (\"" + test.SelectSingleNode(requestXPath).InnerText + "\") expected, but nothing found.";
                }
            }
        }
        public static void StringArrayCompare(string requestXPath, string TargetName, string[] TargetStringArray, ref string logMessage, ref bool passed, XmlNode test)
        {
            if (TargetStringArray == null)
            {
                if (test.SelectNodes(requestXPath).Count != 0)
                {
                    passed = false;
                    logMessage = logMessage + "Wrong number of " + TargetName + "s.";
                }
            }
            else
            {
                if (test.SelectNodes(requestXPath).Count == TargetStringArray.Count())
                {
                    int numberIgnored = test.SelectNodes(requestXPath + "[@ignore=\"1\"]").Count;

                    foreach (string i in TargetStringArray)
                    {
                        //Item
                        if (test.SelectNodes(requestXPath + "[.=\"" + i + "\"]").Count == 0)
                        {
                            if (numberIgnored > 0)
                            {
                                numberIgnored--;
                                logMessage = logMessage + "There is " + TargetName + " = " + i + ".";
                            }
                            else
                            {
                                passed = false;
                                logMessage = logMessage + "There is unexpected " + TargetName + " " + i + ".";
                            }
                        }
                    }
                }
                else
                {
                    passed = false;
                    logMessage = logMessage + "Wrong number of " + TargetName + "s.";
                }
            }
        }
        public static bool IgnoreCheck(string requestXPath, XmlNode test)
        {
            bool res = false;
            if (test.SelectSingleNode(requestXPath + "[@ignore=\"1\"]") != null)
            {
                res = true;
            }
            return res;

        }

        public static void IntCompare(string requestXPath, string TargetName, int TargetInt, ref string logMessage, ref bool passed, XmlNode test)
        {
            if (test.SelectNodes(requestXPath).Count != 0)
            {
                if ((test.SelectSingleNode(requestXPath).NodeType != XmlNodeType.Element) || (test.SelectSingleNode(requestXPath).Attributes["ignore"] == null) ||
                    (test.SelectSingleNode(requestXPath).Attributes["ignore"].InnerXml != "1"))
                {
                    if (Convert.ToInt32(test.SelectSingleNode(requestXPath).InnerText) != TargetInt)
                    {
                        passed = false;
                        logMessage = logMessage + TargetName + " = " + test.SelectSingleNode(requestXPath).InnerText + " but found " + TargetInt.ToString() + ". ";
                    }
                }
                else
                {
                    //passed = true;
                    logMessage = logMessage + TargetName + " = " + TargetInt.ToString() + ". ";
                }
            }
            else
            {
                passed = false;
                logMessage = logMessage + "Unexpected " + TargetName + ". ";
            }
        }
        public static void FloatCompare(string requestXPath, string TargetName, float TargetFloat, ref string logMessage, ref bool passed, XmlNode test)
        {
            if (test.SelectNodes(requestXPath).Count != 0)
            {
                if ((test.SelectSingleNode(requestXPath).Attributes["ignore"] == null) ||
                    (test.SelectSingleNode(requestXPath).Attributes["ignore"].InnerXml != "1"))
                {
                    if (test.SelectSingleNode(requestXPath).InnerText != TargetFloat.ToString())
                    {
                        passed = false;
                        logMessage = logMessage + TargetName + " = " + test.SelectSingleNode(requestXPath).InnerText + " but found " + TargetFloat.ToString() + ". ";
                    }
                }
                else
                {
                    //passed = true;
                    logMessage = logMessage + TargetName + " = " + TargetFloat.ToString() + ". ";
                }
            }
            else
            {
                passed = false;
                logMessage = logMessage + "Unexpected " + TargetName + ". ";
            }
        }
        public static bool Exist(string requestXPath, string TargetName, object TargetObject, ref string logMessage, ref bool passed, XmlNode test)
        {
            bool res = true;

            if (test.SelectNodes(requestXPath).Count != 0)
            {
                if (TargetObject == null)
                {
                    res = false;
                    passed = false;
                    logMessage = logMessage + "Expected " + TargetName + " but not found. ";
                }
            }
            else
            {
                res = false;
                if (TargetObject != null)
                {
                    passed = false;
                    logMessage = logMessage + "Unexpected " + TargetName + ". ";
                }
            }

            return res;
        }

        public static bool Exist2(string requestXPath, string TargetName, bool TargetFlag, ref string logMessage, ref bool passed, XmlNode test)
        {
            bool res = true;
            if (test.SelectNodes(requestXPath).Count != 0)
            {
                if (!TargetFlag)
                {
                    res = false;
                    passed = false;
                    logMessage = logMessage + "Expected " + TargetName + " but not found. ";
                }
            }
            else
            {
                res = false;
                if (TargetFlag)
                {
                    passed = false;
                    logMessage = logMessage + "Unexpected " + TargetName + ". ";
                }
            }
            return res;
        }

        internal static void CompareRealTime(string requestXPath, string TargetName, string TargetString, ref string logMessage, ref bool passed, XmlNode test)
        {
            if (TargetString != null)
            {
                if (test.SelectNodes(requestXPath).Count != 0)
                {
                    int difference = Convert.ToInt32(test.SelectSingleNode(requestXPath).InnerText);
                    System.DateTime expectedDateTime = System.DateTime.UtcNow;
                    expectedDateTime = expectedDateTime.AddSeconds(difference);
                    System.DateTime realDateTime = Convert.ToDateTime(TargetString);
                    if ((realDateTime - expectedDateTime).TotalSeconds > 5)
                    {
                        passed = false;
                        logMessage = logMessage + TargetName + " = now + " + test.SelectSingleNode(requestXPath).InnerText + "sec but found " + TargetString + ". ";
                    }
                }
                else
                {
                    passed = false;
                    logMessage = logMessage + "Unexpected " + TargetName + ". ";
                }
            }
            else
            {
                if (test.SelectNodes(requestXPath).Count != 0)
                {
                    passed = false;
                    logMessage = logMessage + "No expected " + TargetName + ". ";
                }
            }
        }
        internal static void PTZPositionCompare(string requestXPath, string TargetName, PTZ20.PTZVector Position, ref string logMessage, ref bool passed, XmlNode test)
        {
            if (test.SelectNodes(requestXPath).Count != 0)
            {
                if (Position == null)
                {
                    passed = false;
                    logMessage = logMessage + "Expected " + TargetName + " but not found. ";
                }
                else
                {
                    //PanTilt
                    PTZVector2DCompare(requestXPath + "/PanTilt", TargetName + "/PanTilt", Position.PanTilt, ref logMessage, ref passed, test);
                    //Zoom
                    PTZVector1DCompare(requestXPath + "/Zoom", TargetName + "/Zoom", Position.Zoom, ref logMessage, ref passed, test);
                }
            }
            else
            {
                if (Position != null)
                {
                    passed = false;
                    logMessage = logMessage + "Unexpected " + TargetName + ". ";
                }
            }
        }
        private static void PTZVector2DCompare(string requestXPath, string TargetName, PTZ20.Vector2D vector2D, ref string logMessage, ref bool passed, XmlNode test)
        {
            if (test.SelectNodes(requestXPath).Count != 0)
            {
                if (vector2D == null)
                {
                    passed = false;
                    logMessage = logMessage + "Expected " + TargetName + " but not found. ";
                }
                else
                {
                    //x
                    float x = (float)Convert.ToDouble(test.SelectSingleNode(requestXPath).Attributes["x"].Value);
                    if (vector2D.x != x)
                    {
                        passed = false;
                        logMessage = logMessage + "Expected " + TargetName + ".x = " + x.ToString() + " but found " + vector2D.x.ToString() + ". ";
                    }

                    //y
                    float y = (float)Convert.ToDouble(test.SelectSingleNode(requestXPath).Attributes["y"].Value);
                    if (vector2D.x != x)
                    {
                        passed = false;
                        logMessage = logMessage + "Expected " + TargetName + ".y = " + y.ToString() + " but found " + vector2D.y.ToString() + ". ";
                    }
                    //space
                    StringCompare(requestXPath + "/@space", TargetName + "/@space", vector2D.space, ref logMessage, ref passed, test);
                }
            }
            else
            {
                if (vector2D != null)
                {
                    passed = false;
                    logMessage = logMessage + "Unexpected " + TargetName + ". ";
                }
            }
        }
        private static void PTZVector1DCompare(string requestXPath, string TargetName, PTZ20.Vector1D vector1D, ref string logMessage, ref bool passed, XmlNode test)
        {
            if (test.SelectNodes(requestXPath).Count != 0)
            {
                if (vector1D == null)
                {
                    passed = false;
                    logMessage = logMessage + "Expected " + TargetName + " but not found. ";
                }
                else
                {
                    //x
                    float x = (float)Convert.ToDouble(test.SelectSingleNode(requestXPath).Attributes["x"].Value.Replace(".", ","));
                    if (vector1D.x != x)
                    {
                        passed = false;
                        logMessage = logMessage + "Expected " + TargetName + ".x = " + x.ToString() + " but found " + vector1D.x.ToString() + ". ";
                    }

                    //space
                    StringCompare(requestXPath + "/@space", TargetName + "/@space", vector1D.space, ref logMessage, ref passed, test);
                }
            }
            else
            {
                if (vector1D != null)
                {
                    passed = false;
                    logMessage = logMessage + "Unexpected " + TargetName + ". ";
                }
            }
        }
        internal static void PTZSpeedCompare(string requestXPath, string TargetName, PTZ20.PTZSpeed Speed, ref string logMessage, ref bool passed, XmlNode test)
        {
            if (test.SelectNodes(requestXPath).Count != 0)
            {
                if (Speed == null)
                {
                    passed = false;
                    logMessage = logMessage + "Expected " + TargetName + " but not found. ";
                }
                else
                {
                    //PanTilt
                    PTZVector2DCompare(requestXPath + "/PanTilt", TargetName + "/PanTilt", Speed.PanTilt, ref logMessage, ref passed, test);
                    //Zoom
                    PTZVector1DCompare(requestXPath + "/Zoom", TargetName + "/Zoom", Speed.Zoom, ref logMessage, ref passed, test);

                }
            }
            else
            {
                if (Speed != null)
                {
                    passed = false;
                    logMessage = logMessage + "Unexpected " + TargetName + ". ";
                }
            }
        }

    }
}
