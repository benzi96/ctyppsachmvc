using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctyppsachmvc.Models
{
    public class chitietdoanhthu
    {
        public DateTime ngaygiaodich { get; set; }
        public string tensach { get; set; }
        public int soluongdaban { get; set; }
        public decimal thanhtien { get; set; }
        public decimal sotienphaitrachonxb { get; set; }
        public decimal loinhuan { get; set; }

        public chitietdoanhthu(ctdmsdb ctdmsdb)
        {
            ngaygiaodich = (DateTime)ctdmsdb.danhmucsachdaban.thoigian;
            tensach = ctdmsdb.sach.tensach;
            soluongdaban = (int)ctdmsdb.soluong;
            thanhtien = (decimal)(ctdmsdb.soluong * ctdmsdb.sach.giaxuat);
            sotienphaitrachonxb = (decimal)(ctdmsdb.soluong * ctdmsdb.sach.gianhap);
            loinhuan = thanhtien - sotienphaitrachonxb;
        }
    }
}