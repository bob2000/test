using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MassTransit;

namespace MvcMTApp.Services
{
    public class OrderService : IService
    {
        IServiceBus _bus;

        public OrderService(IServiceBus bus)
        {
            _bus = bus;
        }

        public void Start()
        {
            _bus = ServiceBusFactory.New(x =>
            {
                x.UseRabbitMq();
                x.ReceiveFrom("rabbitmq://localhost/learningmt_orderservice");
            });
        }

        public void Stop()
        {
            _bus.Dispose();
        }
    }
}