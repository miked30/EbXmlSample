using System;
using Shared;
using System.ServiceModel;

namespace EbXmlClient
{
    class Program
    {
        static void Main()
        {
            var channelFactory = new ChannelFactory<ITestEndpoint>("TestEndpoint");
            var channel = channelFactory.CreateChannel();

            if (channel == null)
                throw new ApplicationException("The channel wasn't created properly");

            try
            {
                Console.WriteLine("Please type a value to send to the server and press enter");
                var value = Console.ReadLine();

                var response = channel.DoSomething(new DoSomethingType
                    {
                        AnObject = new PayloadType
                            {
                                ImportantValue = value,
                            }
                    });

                Console.WriteLine("Received a response from the server");
                Console.WriteLine("The value received was '{0}'.", response.ResponseObject.Ok);
            }
            finally
            {
                var clientChannel = (IClientChannel) channel;
                if (clientChannel.State == CommunicationState.Faulted)
                    clientChannel.Abort();
                else
                    clientChannel.Close();
            }

            Console.WriteLine();
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
    }
}
