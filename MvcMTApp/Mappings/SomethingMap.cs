using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using MvcMTApp.Models.DB;

namespace MvcMTApp.Mappings
{
    public class SomethingMap : ClassMap<Something>
    {
        public SomethingMap()
        {
            Id(i => i.Id).Access.CamelCaseField();
        }
    }
}