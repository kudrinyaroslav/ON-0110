using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Xml;
using System.ServiceModel.Description;

namespace DUT.CameraWebService.Media2SVC
{
    class DispatchByBodyElementOperationSelector  : IDispatchOperationSelector

    {

        List<string> dispatchDictionary;

        string defaultOperationName;



        public DispatchByBodyElementOperationSelector(List<string> dispatchDictionary, string defaultOperationName)

        {

            this.dispatchDictionary = dispatchDictionary;

            this.defaultOperationName = defaultOperationName;

        }

 

        #region IDispatchOperationSelector Members

 

        private Message CreateMessageCopy(Message message, XmlDictionaryReader body)

        {

            Message copy = Message.CreateMessage(message.Version,message.Headers.Action,body);

            copy.Headers.CopyHeaderFrom(message,0);

            copy.Properties.CopyProperties(message.Properties);

            return copy;

        }

 

        public string SelectOperation(ref System.ServiceModel.Channels.Message message)

        {

            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();

            XmlQualifiedName lookupQName = new XmlQualifiedName(bodyReader.LocalName, bodyReader.NamespaceURI);

            message = CreateMessageCopy(message,bodyReader);

 

            var operationName = dispatchDictionary.FirstOrDefault(e => lookupQName.Name.ToLower().Contains(e.ToLower()));            

 

            if(operationName!=null)

            {

                return operationName;

            }

            

            return defaultOperationName;            

        }

 

        #endregion

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]

    sealed class DispatchByBodyElementBehaviorAttribute : Attribute, IContractBehavior
    {

        #region IContractBehavior Members



        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

            return;

        }



        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {

            return;

        }



        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.DispatchRuntime dispatchRuntime)
        {

            List<string> operations = new List<string>();

            foreach (OperationDescription operationDescription in contractDescription.Operations)
            {

                operations.Add(operationDescription.Name);

            }



            dispatchRuntime.OperationSelector = new DispatchByBodyElementOperationSelector(operations, dispatchRuntime.UnhandledDispatchOperation.Name);

        }



        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {

        }



        #endregion

    }

    public class DispatchByBodyElementServiceBehaviorAttribute : Attribute, IServiceBehavior
    {

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

            //

        }



        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {

            //

            foreach (var endpoint in serviceDescription.Endpoints)
            {

                endpoint.Contract.Behaviors.Add(new DispatchByBodyElementBehaviorAttribute());

            }

        }



        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {

            //

        }

    }

 

}
