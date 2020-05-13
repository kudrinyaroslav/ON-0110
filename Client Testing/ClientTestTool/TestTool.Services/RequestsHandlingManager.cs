using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Services
{
    public class RequestsHandlingManager
    {
        public RequestsHandlingManager(ILogger logger, IEnumerable<BaseService> services)
        {
            logger.RequestReceived += new TrafficEvent(logger_RequestReceived);
            logger.ResponseSent += new TrafficEvent(logger_ResponseSent);

            foreach (BaseService service in services)
            {
                service.MethodStarted += new ServiceEvent(service_MethodStarted);
                service.MethodCompleted += new ServiceEvent(service_MethodCompleted);
            }
        }




        public event RequestProcessedEvent RequestProcessed;
        public event ServiceEvent MethodStarted;
        public event ServiceEvent MethodCompleted;
        public event TrafficEvent RequestReceived;
        public event TrafficEvent ResponseSent;

        private bool _requestRecognized = false;
        private string _currentMethod;
        private string _currentservice;
        private string _request;
        private string _response;

        void service_MethodStarted(BaseService service, string message)
        {
            _requestRecognized = true;
            _currentMethod = message;
            _currentservice = service.GetServiceName();
            if (MethodStarted != null)
            {
                MethodStarted(service, message);
            }
        }

        void service_MethodCompleted(BaseService service, string message)
        {
            if (MethodCompleted != null)
            {
                MethodCompleted(service, message);
            }
        }

        void logger_ResponseSent(string log)
        {
            _response = log;
            if (RequestProcessed != null)
            {
                RequestProcessingLog trace = new RequestProcessingLog();
                trace.Time = DateTime.Now;
                trace.Request = _request;
                trace.Response = _response;
                
                if (_requestRecognized)
                {
                    trace.Service = _currentservice;
                    trace.Method = _currentMethod;
                }
                else
                {
                    trace.Method = "NOT HANDLED";
                }

                RequestProcessed(trace);
            }

            if (ResponseSent != null)
            {
                ResponseSent(log);
            }
        }

        
        void logger_RequestReceived(string log)
        {
            _request = log;
            _requestRecognized = false;

            if (RequestReceived != null)
            {
                RequestReceived(log);
            }
        }

    }
}
