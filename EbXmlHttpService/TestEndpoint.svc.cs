using Shared;

namespace EbXmlHttpService
{
    public class TestEndpoint : ITestEndpoint
    {
        public AcknowledgementResponseType DoSomething(DoSomethingType doSomething)
        {
            return new AcknowledgementResponseType
                {
                    ResponseObject = new ResponseType
                        {
                            Ok = doSomething.AnObject.ImportantValue == "42",
                        }
                };
        }
    }
}
