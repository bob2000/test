using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using MvcMTApp.Models.DB;

namespace MvcMTApp.Mappings
{
    public class OpinionsMap : ClassMap<Opinion>
    {
        public OpinionsMap()
        {
            Id(i => i.Opinion_Id);
            Map(i => i.Content);
            Map(i => i.Rating);
            References(i => i.Book);
        }
    }
}