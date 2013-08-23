using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MassTransit;
using MvcMTApp.Messages;

namespace MvcMTApp.Consumers
{
    public class SubmitOrderConsumer : Consumes<SubmitOrder>.Context
    {
        public void Consume(IConsumeContext<SubmitOrder> context)
        {
        }
    }
}