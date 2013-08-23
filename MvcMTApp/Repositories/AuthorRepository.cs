using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using MvcMTApp.Models.DB;
using MvcMTApp.App_Start;

namespace MvcMTApp.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private ISession session = null;

        public AuthorRepository(ICustomSession<NhTestConfig> session)
        {
            this.session = session.GetSession();
        }

        public void AddUser(Author author)
        {
            session.Save(author);
        }

        public int GetSessionHasCode()
        {
            return session.GetHashCode();
        }
    }
}