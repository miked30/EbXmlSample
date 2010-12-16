using System.ServiceModel;
using Shared.ServiceModel;

namespace Shared
{
    [ServiceContract(Namespace="http://my.ebXML.schema.com")]
    public interface ITestEndpoint
    {
        [OperationContract(Action = "DoSomething")]
        AcknowledgementResponseType DoSomething(DoSomethingType doSomething);
    }
}