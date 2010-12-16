using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Shared.ServiceModel
{
    public class EbXmlOperationSelector : IDispatchOperationSelector
    {
        private readonly string _namespace;

        public EbXmlOperationSelector(ContractDescription contractDescription)
        {
            _namespace = contractDescription.Namespace;
        }

        public string SelectOperation(ref Message message)
        {
            var messageBuffer = message.CreateBufferedCopy(16384);
            // Determine the name of the root node of the message
            using (var copyMessage = messageBuffer.CreateMessage())
            using (var reader = copyMessage.GetReaderAtBodyContents())
            {
                // Move to the first element
                reader.MoveToContent();

                if (reader.NamespaceURI != _namespace)
                    throw new InvalidOperationException("The namespace of the incoming message does not match the namespace of the endpoint contract.");

                // The root element name is the operation name
                var action = reader.LocalName;

                // Reset the message for subsequent processing
                message = messageBuffer.CreateMessage();

                // Return the name of the action to execute
                return action;
            }
        }
    }
}