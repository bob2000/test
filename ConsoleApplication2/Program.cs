using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit;
using System.Threading;
using MassTransit.Pipeline;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            var consumerBus = ServiceBusFactory.New(b =>
            {
                b.UseRabbitMq();
                b.ReceiveFrom("rabbitmq://localhost/mtloss_consumer");
            });
            var publisherBus = ServiceBusFactory.New(b =>
            {
                b.UseRabbitMq();
                b.ReceiveFrom("rabbitmq://localhost/mtloss");
                b.EnableMessageTracing();
            });
            //consumerBus.SubscribeConsumer<MessageConsumer>();
            consumerBus.SubscribeConsumer(() => new MessageConsumer("consumerBus"));
            //publisherBus.SubscribeConsumer(() => new MessageConsumer("publisherBus"));
            for (int i = 1; i <= 10; i++)
                publisherBus.Publish(new SimpleMessage() { CorrelationId = Guid.NewGuid(), Message = string.Format("This is message {0}", i) });
            Console.WriteLine("Press ENTER Key to see how many you consumed");
            Console.ReadLine();
            Console.WriteLine("We consumed {0} simple messages. Press Enter to terminate the applicaion.", MessageConsumer.Count);
            Console.ReadLine();
            consumerBus.Dispose();
            publisherBus.Dispose();
        }
    }

    public class CF : IConsumerFactory<MessageConsumer>
    {
        public IEnumerable<Action<IConsumeContext<TMessage>>> GetConsumer<TMessage>(
            IConsumeContext<TMessage> context, 
            InstanceHandlerSelector<MessageConsumer, TMessage> selector) where TMessage : class
        {
            throw new NotImplementedException();
        }
    }

    public interface ISimpleMessage : CorrelatedBy<Guid>
    {
        string Message { get; }
    }
    public class SimpleMessage : ISimpleMessage
    {
        public Guid CorrelationId { get; set; }
        public string Message { get; set; }
    }
    public class MessageConsumer : Consumes<ISimpleMessage>.All
    {
        private static int ConsumerIdCounter = 0;
        private int ConsumerId = 0;
        private string ConsumerName = string.Empty;

        public MessageConsumer(string name)
        {
            ConsumerName = name;
            //Interlocked.Increment(ref ConsumerIdCounter);
            ConsumerId = ++ConsumerIdCounter;
            //Console.WriteLine("New MessageConsumer: " + ConsumerIdCounter + " | " + ConsumerName);
        }

        public static int Count = 0;
        public void Consume(ISimpleMessage message)
        {
            Console.WriteLine(message.Message + " | consumer id: " + ConsumerId + " | " + ConsumerName);
            Interlocked.Increment(ref Count);
        }
    }

    /////////////////////////////////

    //class Program
    //{
    //    internal static bool[] msgReceived = new bool[10];
    //    static void Main(string[] args)
    //    {
    //        var consumerBus = ServiceBusFactory.New(b =>
    //        {
    //            b.UseRabbitMq();
    //            b.ReceiveFrom("rabbitmq://localhost/mtloss");
    //        });
    //        var publisherBus = ServiceBusFactory.New(b =>
    //        {
    //            b.UseRabbitMq();
    //            b.ReceiveFrom("rabbitmq://localhost/mtloss");
    //        });
    //        //publisherBus.SubscribeConsumer(() => new MessageConsumer2());
    //        consumerBus.SubscribeConsumer(() => new MessageConsumer());
    //        for (int i = 0; i < 10; i++)
    //            publisherBus.Publish(new SimpleMessage() { CorrelationId = Guid.NewGuid(), MsgId = i });
    //        Console.WriteLine("Press ENTER Key to see how many you consumed");
    //        Console.ReadLine();

    //        //Console.WriteLine("MessageConsumer consumed {0} simple messages.", MessageConsumer.Count);
    //        //Console.WriteLine("MessageConsumer2 consumed {0} simple messages.", MessageConsumer2.Count);
    //        //Console.WriteLine("Press Enter to terminate the applicaion.");

    //        Console.WriteLine("We consumed {0} simple messages. Press Enter to terminate the applicaion.",
    //                          MessageConsumer.Count);
    //        for (int i = 0; i < 10; i++)
    //            if (msgReceived[i])
    //                Console.WriteLine("Received {0}", i);

    //        Console.ReadLine();
    //        consumerBus.Dispose();
    //        publisherBus.Dispose();

    //    }
    //}
    //public interface ISimpleMessage : CorrelatedBy<Guid>
    //{
    //    int MsgId { get; }
    //}
    //public class SimpleMessage : ISimpleMessage
    //{
    //    public Guid CorrelationId { get; set; }
    //    public int MsgId { get; set; }
    //}
    //public class MessageConsumer : Consumes<ISimpleMessage>.All
    //{
    //    public static int Count = 0;
    //    public void Consume(ISimpleMessage message)
    //    {
    //        Program.msgReceived[message.MsgId] = true;
    //        //Console.WriteLine("MessageConsumer: message id = " + message.MsgId);
    //        Interlocked.Increment(ref Count);
    //        //Thread.Sleep(2000);
    //    }
    //}

    ////public interface ISimpleMessage2 : CorrelatedBy<Guid>
    ////{
    ////    int MsgId { get; }
    ////}
    ////public class SimpleMessage2 : ISimpleMessage2
    ////{
    ////    public Guid CorrelationId { get; set; }
    ////    public int MsgId { get; set; }
    ////}
    ////public class MessageConsumer2 : Consumes<ISimpleMessage>.All
    ////{
    ////    public static int Count = 0;
    ////    public void Consume(ISimpleMessage message)
    ////    {
    ////        Program.msgReceived[message.MsgId] = true;
    ////        Console.WriteLine("MessageConsumer2: message id = " + message.MsgId);
    ////        Interlocked.Increment(ref Count);
    ////        //Thread.Sleep(2000);
    ////    }
    ////}
}
