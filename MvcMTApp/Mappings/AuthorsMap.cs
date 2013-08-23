using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using MvcMTApp.Models.DB;

namespace MvcMTApp.Mappings
{
    public class AuthorsMap : ClassMap<Author>
    {
        public AuthorsMap()
        {
            Table("authors");
            Id(i => i.Id).Column("author_id").Access.CamelCaseField();
            Map(i => i.Name);
        }
    }
}