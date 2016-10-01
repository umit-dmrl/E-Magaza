using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCinema.ViewModel
{
    public class SeoViewModel
    {
        [Display(Name="Dosya Seç")]
        [Required(ErrorMessage="Lütfen Bir Dosya Seçiniz")]
        public HttpPostedFileBase dosya { get; set; }

        public bool IsSuccess { get; set; }
        public string AjaxMessage { get; set; }
    }
}