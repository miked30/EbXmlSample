using System.ServiceModel.Channels;
using PoxHttpCertificateService.library;

namespace PoxHttpCertificateService
{
    public class EbXmlMessageActionParser : ChannelMessageInterceptor
    {
        public override void OnReceive(ref Message msg)
        {
            var messageBuffer = msg.CreateBufferedCopy(16384);
            // Determine the name of the root node of the message
            using (var copyMessage = messageBuffer.CreateMessage())
            using (var reader = copyMessage.GetReaderAtBodyContents())
            {
                // Move to the first element
                reader.MoveToContent();

                // The root element name is the operation name
                var elementNamespace = reader.NamespaceURI;
                var action = reader.LocalName;
                //var bodyContent = reader.ReadInnerXml();

                // Recreate the request without the wrapping element and add action
                //msg = Message.CreateMessage(MessageVersion.None, action, XmlReader.Create(new StringReader(bodyContent)));
                msg = messageBuffer.CreateMessage();
                msg.Headers.Action = action;
                msg.Headers.To = copyMessage.Headers.To;
                msg.Properties.Add("Namespace", elementNamespace);
                msg.Properties.Add("Action", action);
            }
        }

        public override ChannelMessageInterceptor Clone()
        {
            return new EbXmlMessageActionParser();
        }
    }
}