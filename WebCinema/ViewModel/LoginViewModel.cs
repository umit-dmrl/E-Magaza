using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCinema.ViewModel
{
    public class LoginViewModel
    {
        [Display(Name="E-Posta Adresiniz")]
        [Required(ErrorMessage = "E-Posta Adresinizi Girmediniz!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Geçersiz Bir E-Posta Adresi Girdiniz!")]
        [EmailAddress]
        [StringLength(150, ErrorMessage = "Adınız En Fazla 150 En Az 10 Karakter Olabilir", MinimumLength = 10)]
        public string eposta { get; set; }

        [Display(Name = "Parolanızı Girin")]
        [Required(ErrorMessage = "Giriş Parolanızı Giriniz!")]
        [DataType(DataType.Password)]
        public string parola { get; set; }

        public string mesaj { get; set; }
    }
}