using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace WebCinema.ViewModel
{
    public class KategoriModel
    {
        public string kategoriAdi { get; set; }
        public int? Page { get; set; }
        public IPagedList<KategoriListModel> Kategoriler { get; set; }
    }
}