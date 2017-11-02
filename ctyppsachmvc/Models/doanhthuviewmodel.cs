using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctyppsachmvc.Models
{
    public class doanhthuviewmodel
    {
        public List<chitietdoanhthu> ctdt { get; set; }
        public decimal doanhthu { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
    }
}