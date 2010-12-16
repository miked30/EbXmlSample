using System;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Xml;
using System.Xml.Serialization;

namespace Shared.ServiceModel
{
    public class SimpleXmlFormatter : IDispatchMessageFormatter, IClientMessageFormatter
    {
        private readonly Type _requestMessageType;
        private readonly Type _responseMessageType;

        public SimpleXmlFormatter(OperationDescription operationDescription)
        {
            // Get the request message type
            var parameters = operationDescription.SyncMethod.GetParameters();
            if (parameters.Length != 1)
                throw new InvalidDataContractException("The SimpleXmlFormatter will only work with a single parameter for an operation which is the type of the incoming message contract.");
            _requestMessageType = parameters[0].ParameterType;

            // Get the response message type
            _responseMessageType = operationDescription.SyncMethod.ReturnType;
        }

        public void DeserializeRequest(Message message, object[] parameters)
        {
            var serializer = new XmlSerializer(_requestMessageType);
            parameters[0] = serializer.Deserialize(message.GetReaderAtBodyContents());
        }

        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            return Message.CreateMessage(MessageVersion.None, _responseMessageType.Name,
                                         new SerializingBodyWriter(_responseMessageType, result));
        }

        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            return Message.CreateMessage(MessageVersion.None, _requestMessageType.Name,
                                         new SerializingBodyWriter(_requestMessageType, parameters[0]));
        }

        public object DeserializeReply(Message message, object[] parameters)
        {
            var serializer = new XmlSerializer(_responseMessageType);
            return serializer.Deserialize(message.GetReaderAtBodyContents());
        }

        private class SerializingBodyWriter : BodyWriter
        {
            private readonly Type _typeToSerialize;
            private readonly object _objectToEncode;

            public SerializingBodyWriter(Type typeToSerialize, object objectToEncode) : base(false)
            {
                _typeToSerialize = typeToSerialize;
                _objectToEncode = objectToEncode;
            }

            protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
            {
                writer.WriteStartDocument();
                var serializer = new XmlSerializer(_typeToSerialize);
                serializer.Serialize(writer, _objectToEncode);
                writer.WriteEndDocument();
            }
        }
    }
}