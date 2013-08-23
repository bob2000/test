using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;

namespace MvcMTApp.Repositories
{
    public class FishReposiotry : IFishRepository
    {
        private ISession session = null;

        public FishReposiotry(ISession session)
        {
            this.session = session;
        }

        public int GetSessionHasCode()
        {
            throw new NotImplementedException();
        }
    }
}