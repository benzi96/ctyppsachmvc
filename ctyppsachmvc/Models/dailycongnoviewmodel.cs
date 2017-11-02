using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctyppsachmvc.Models
{
    public class dailycongnoviewmodel
    {
        public DateTime dt { get; set; }
        public List<daily> daily { get; set; }
    }
}