﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Common.Attributes;
using TestTool.Tests.Common.TestBase;
using TestTool.Tests.Common.TestEngine;
using TestTool.Tests.Common.Enums;
using TestTool.Proxies.Media;
using System.Net;
using System.IO;

namespace TestTool.Tests.TestCases.TestSuites
{

    [TestClass]
    class MediaStreamingTestSuite : Base.MediaTest
    {
        public MediaStreamingTestSuite(TestLaunchParam param)
            : base(param)
        {
        }

        private const string PATH = "Media Configuration\\Media Streaming";
        
        [Test(Name = "SNAPSHOT URI",
            Path = PATH,
            Order = "07.06.01",
            Version = 1.02,
            Services = new Service[] { Service.Device, Service.Media },
            RequirementLevel = RequirementLevel.Should)]
        public void NvtSnapshotUriTest()
        {
            RunTest(() =>
            {
                Profile[] profiles = GetProfiles();
                Assert((profiles != null && profiles.Length > 0), "No profile available", "Check if DUT returned at least one profile");

                Profile mediaProfile = null;
                foreach (Profile profile in profiles)
                {
                    if (profile.VideoEncoderConfiguration != null
                        && profile.VideoSourceConfiguration != null)
                    {
                        mediaProfile = profile;
                        break;
                    }
                }

                Assert(mediaProfile != null,
                    "Profile with Video Encoder configuration and Video Source configuration not found",
                    "Check if media profile with video source and video encoder is present");

                MediaUri response = GetSnapshotUri(mediaProfile.token);

                Assert(response != null,
                    "Response returned is null",
                    "Check that response is not null");

                Assert(response.Uri.IsValidUrl(),
                    "URL provided is not valid",
                    "Check that MediaUri field contains valid URL", string.Format("URI: {0}", response.Uri));


                // HTTP GET

                HttpWebResponse httpResponse = null;

                RunStep(() =>
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(response.Uri);
                    NetworkCredential credential = new NetworkCredential(_username, _password);
                    request.Credentials = credential;
                    httpResponse = (HttpWebResponse)request.GetResponse();

                }, "Invoke HTTP GET request on snapshot URI");


                Assert(httpResponse.ContentType.ToLower() == "image/jpeg",
                    "ContentType is not image/jpeg",
                    "Check ContentType header",
                    string.Format("ContentType: {0}", httpResponse.ContentType));

                Assert(httpResponse.StatusCode == HttpStatusCode.OK, 
                    "HTTP Status is not '200 OK'", 
                    "Check HTTP status code", 
                    string.Format("HTTP Status: {0} {1}", httpResponse.StatusCode, httpResponse.StatusDescription));
                




                Stream receiveStream = httpResponse.GetResponseStream();

                MemoryStream memoryStream = new MemoryStream();

                int bufSize = 1024;

                byte[] buffer = new byte[bufSize];
                int bytes = receiveStream.Read(buffer, 0, bufSize);
                while (bytes > 0)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Next {0} bytes", bytes));

                    memoryStream.Write(buffer, 0, bytes);
                    bytes = receiveStream.Read(buffer, 0, bufSize);
                }
                
                httpResponse.Close();

                Assert(memoryStream.GetBuffer().IsJpeg(), 
                    "Information returned is not JPEG image", 
                    "Validate JPEG image");
            });

        }


    }
}
