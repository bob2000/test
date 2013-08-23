using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcMTApp.Models.DB;

namespace MvcMTApp.Repositories
{
    public interface IAuthorRepository : IRepository
    {
        void AddUser(Author author);
    }
}