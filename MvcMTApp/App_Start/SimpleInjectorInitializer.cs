[assembly: WebActivator.PostApplicationStartMethod(typeof(MvcMTApp.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace MvcMTApp.App_Start
{
    using System.Reflection;
    using System.Web.Mvc;

    using SimpleInjector;
    using SimpleInjector.Integration.Web.Mvc;
    using MassTransit;
    using NHibernate;
    using FluentNHibernate.Cfg.Db;
    using FluentNHibernate.Cfg;
    using MvcMTApp.Models.DB;
    using MvcMTApp.Services;
    using System;
    using System.Threading;
    using MvcMTApp.UOW;
    using MvcMTApp.Repositories;
    
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            // Did you know the container can diagnose your configuration? Go to: http://bit.ly/YE8OJj.
            var container = new Container();
            
            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterMvcAttributeFilterProvider();
            container.Verify();
            
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
     
        private static void InitializeContainer(Container container)
        {
            // #error Register your services here (remove this line).

            // For instance:
            // container.Register<IUserRepository, SqlUserRepository>();

            RegisterRabbitMQ(container);
            RegisterNHibernate(container);

            container.Register<IService, OrderService>();
        }

        private static void RegisterRabbitMQ(Container container)
        {
            container.RegisterSingle<IServiceBus>(ServiceBusFactory.New(
                    sbc =>
                    {
                        sbc.UseRabbitMq();
                        sbc.ReceiveFrom("rabbitmq://localhost/test_queue");
                        sbc.SupportJsonSerializer();
                    }
                ));

            container.RegisterInitializer<MessageConsumer>( i => i.ConsumerName = "dupa");
        }

        private static void RegisterNHibernate(Container container)
        {
            //ISessionFactory nhibernateSessionFactory = Fluently.Configure()
            //            .Database(MsSqlConfiguration.MsSql2008
            //            .ConnectionString(c => c.FromConnectionStringWithKey("nhtest_db")).ShowSql())
            //            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Book>())
            //            .BuildSessionFactory();

            //container.RegisterSingle<ISessionFactory>(() => nhibernateSessionFactory);
            //container.RegisterPerWebRequest<ISession>(() => nhibernateSessionFactory.OpenSession(), true);
            //container.RegisterPerWebRequest<IStatelessSession>(() => nhibernateSessionFactory.OpenStatelessSession(), true);

            container.RegisterSingle<ICustomSessionFactory<NhTestConfig>, CustomSessionFactory<NhTestConfig>>();
            container.RegisterPerWebRequest<ICustomSession<NhTestConfig>, CustomSession<NhTestConfig>>();

            container.RegisterSingle<ICustomSessionFactory<NhTestAnotherConfig>, CustomSessionFactory<NhTestAnotherConfig>>();
            container.RegisterPerWebRequest<ICustomSession<NhTestAnotherConfig>, CustomSession<NhTestAnotherConfig>>();

            container.Register<IAuthorRepository, AuthorRepository>();
            container.Register<ISomethingRepository, SomethingRepository>();

            //container.Register<INHibernateSession<NH_Entities1>>();
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
            public string ConsumerName { get; set; }

            public MessageConsumer()
            {
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
    }
}