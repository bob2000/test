using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMTApp.Models.DB
{
    public class Opinion
    {
        public virtual int Opinion_Id { get; set; }
        public virtual string Content { get; set; }
        public virtual int Rating { get; set; }
        public virtual Book Book { get; set; }
    }
}