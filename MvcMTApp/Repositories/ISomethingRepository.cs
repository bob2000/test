using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcMTApp.Models.DB;

namespace MvcMTApp.Repositories
{
    public interface ISomethingRepository : IRepository
    {
        void AddSomething(Something something);
    }
}