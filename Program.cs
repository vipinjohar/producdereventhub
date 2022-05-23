using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerEventHub
{
    class Program
    {

        static string eventHubName = "firsteventhub";
        static string connectionString = "Endpoint=sb://droisysevenhub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=vlXWP6DuhKZS7k/m7OXle7Vx1ZokyNAgIYLnd1STD5Q=";
        // The Event Hubs client types are safe to cache and use as a singleton for the lifetime
        // of the application, which is best practice when events are being published or read regularly.
        static EventHubProducerClient producerClient;
        // number of events to be sent to the event hub
        private const int numOfEvents = 200000;

        static async Task Main()
        {
            // Create a producer client that you can use to send events to an event hub
            producerClient = new EventHubProducerClient(connectionString, eventHubName);

            // Create a batch of events 
            using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

            for (int i = 1; i <= numOfEvents; i++)
            {
                var message = "Data from prodcucer application  " + i;
                if (eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(message))))
                {
                    
                    await producerClient.SendAsync(eventBatch);
                    Console.WriteLine($"A batch of {message} events has been published.");

                    Thread.Sleep(200);
                    // if it is too large for the batch
                    //throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
                }
            }

            try
            {
                // Use the producer client to send the batch of events to the event hub
                //await producerClient.SendAsync(eventBatch);
                //Console.WriteLine($"A batch of {numOfEvents} events has been published.");
            }
            finally
            {
                await producerClient.DisposeAsync();
            }
        }
        //static void SendingRandomMessages()
        //{
        //    var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
        //    while (true)
        //    {
        //        try
        //        {
        //            var message = Guid.NewGuid().ToString();
        //            Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, message);
        //            eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes(message)));
        //        }
        //        catch (Exception exception)
        //        {
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            Console.WriteLine("{0} > Exception: {1}", DateTime.Now, exception.Message);
        //            Console.ResetColor();
        //        }
        //        Thread.Sleep(200);
        //    }
        //}
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Hello World!");

        //    Console.WriteLine("Press Ctrl-C to stop the sender process");
        //    Console.WriteLine("Press Enter to start now");
        //    Console.ReadLine();
        //    SendingRandomMessages();
        //}
    }
}
