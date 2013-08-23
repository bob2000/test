using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MassTransit;

namespace MvcMTApp.Messages
{
    public interface SubmitOrder : CorrelatedBy<Guid>
    {
        DateTime SubmitDate { get; }
        string CustomerNumber { get; }
        string OrderNumber { get; }
    }
}