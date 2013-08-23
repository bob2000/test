using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMTApp.Repositories
{
    public interface IRepository
    {
        int GetSessionHasCode();
    }
}