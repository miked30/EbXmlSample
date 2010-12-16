using System.IO;
using System.Xml;
using PoxHttpCertificateService.library;

namespace PoxHttpCertificateService
{
	class EbXmlMessageActionParserElement : InterceptingElement
	{
		protected override ChannelMessageInterceptor CreateMessageInterceptor()
		{
			return new EbXmlMessageActionParser();
		}
	}
}