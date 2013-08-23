using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMTApp.Services
{
    public interface IService
    {
        void Start();
        void Stop();
    }
}