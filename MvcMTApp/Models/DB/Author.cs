using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMTApp.Models.DB
{
    public class Author
    {
        public Author() { }

        public Author(string name)
        {
            this.Name = name;
        }

        private int id;
        public virtual int Id { get { return id; } }
        //public virtual int Author_Id { get; private set; }
        public virtual string Name { get; protected internal set; }


    }
}