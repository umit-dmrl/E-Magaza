using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace WebCinema.ViewModel
{
    public class UrunAraViewModel
    {
        
        public string UrunAdi { get; set; }
        public string UrunKodu { get; set; }
        public int? Page { get; set; }
        public IPagedList<UrunListModel> Urunler { get; set; }
    }
}