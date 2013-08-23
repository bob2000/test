using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using MvcMTApp.Models.DB;

namespace MvcMTApp.Mappings
{
    public class BooksMap : ClassMap<Book>
    {
        public BooksMap()
        {
            Table("Books");
            Id(i => i.Book_Id);
            Map(i => i.Book_Name);
            
        }
    }
}