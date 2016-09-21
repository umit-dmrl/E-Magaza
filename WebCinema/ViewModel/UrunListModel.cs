using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCinema.ViewModel
{
    public class UrunListModel
    {
        public int UrunID { get; set; }
        public string UrunKodu { get; set; }
        public string UrunResmi { get; set; }
        public string UrunAdi { get; set; }
        public string UrunFiyati { get; set; }
        public string UrunStokAdeti { get; set; }
        public string UrunOnayi { get; set; }
    }
}