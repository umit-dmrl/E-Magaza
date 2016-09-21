using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace WebCinema.ViewModel
{
    public class ResimSecimListViewModel
    {
        public int resimID { get; set; }
        public string resim { get; set; }
        public bool secim { get; set; }
    }
}