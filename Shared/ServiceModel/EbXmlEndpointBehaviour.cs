using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Shared.ServiceModel
{
    public class EbXmlEndpointBehaviour : IEndpointBehavior
    {
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.ContractFilter = new MatchAllMessageFilter();
            endpointDispatcher.DispatchRuntime.OperationSelector = new EbXmlOperationSelector(endpoint.Contract);
            ApplyCustomFormatterToOperations(endpoint);
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            ApplyCustomFormatterToOperations(endpoint);
        }

        private static void ApplyCustomFormatterToOperations(ServiceEndpoint endpoint)
        {
            foreach (var operation in endpoint.Contract.Operations)
            {
                // Remove an of the standard formatters that might have been applied
                operation.Behaviors.Remove<DataContractSerializerOperationBehavior>();
                operation.Behaviors.Remove<XmlSerializerOperationBehavior>();

                // Add our new formatter for ebXml messages
                if (!operation.Behaviors.Contains(typeof(SimpleXmlFormatterBehaviour)))
                    operation.Behaviors.Add(new SimpleXmlFormatterBehaviour());
            }
        }
    }
}