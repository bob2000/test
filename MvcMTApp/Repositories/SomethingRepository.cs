using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using MvcMTApp.Models.DB;
using MvcMTApp.App_Start;

namespace MvcMTApp.Repositories
{
    public class SomethingRepository : ISomethingRepository
    {
        private ISession session = null;

        public SomethingRepository(ICustomSession<NhTestAnotherConfig> session)
        {
            this.session = session.GetSession();
        }

        public void AddSomething(Something something)
        {
            session.Save(something);
        }

        public int GetSessionHasCode()
        {
            return session.GetHashCode();
        }
    }
}