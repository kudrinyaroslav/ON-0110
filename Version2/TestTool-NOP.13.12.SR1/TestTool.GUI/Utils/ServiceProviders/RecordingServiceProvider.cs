using System;
using System.Collections.Generic;
using TestTool.HttpTransport.Interfaces;
using TestTool.Proxies.Onvif;
using TestTool.Tests.Common.Transport;
using TestTool.Tests.Common.CommonUtils;
using System.Xml;
using System.ServiceModel.Channels;
using TestTool.HttpTransport;
using System.ServiceModel;
using TestTool.Tests.Definitions.Interfaces;

namespace TestTool.GUI.Utils
{
    class RecordingServiceProvider : BaseServiceProvider<RecordingPortClient, RecordingPort>
    {
        public RecordingServiceProvider(string serviceAddress, int messageTimeout) : 
            base(serviceAddress, messageTimeout)
        {
            EnableLogResponse = false;
        }

        public override RecordingPortClient CreateClient(System.ServiceModel.Channels.Binding binding, 
            System.ServiceModel.EndpointAddress address)
        {
            return new RecordingPortClient(binding, address);
        }

        public List<GetRecordingsResponseItem> GetRecordings()
        {
            List<GetRecordingsResponseItem> list = new List<GetRecordingsResponseItem>();
            GetRecordingsResponseItem[] response = new GetRecordingsResponseItem[0];

            if (Security == Security.None)
            {
                Action action = ConstructSecurityTolerantAction(
                        () =>
                        {
                            response = Client.GetRecordings();
                        }
                        );

                action();

            }
            else
            {
                response = Client.GetRecordings();
            }
            list.AddRange(response);
            return list;
        }

    }
}
