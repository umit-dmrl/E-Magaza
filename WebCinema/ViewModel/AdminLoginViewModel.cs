using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCinema.ViewModel
{
    public class AdminLoginViewModel
    {
        [Display(Name = "E-Posta Adresiniz")]
        [StringLength(150, ErrorMessage = "E-Posta Adresi En Fazla 150 En Az 10 Karakter Olmalı!", MinimumLength = 10)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Geçersiz Bir E-Posta Adresi Girdiniz!")]
        [EmailAddress]
        [Required(ErrorMessage="Kayıt Olduğunuz E-Posta Adresini Giriniz!")]
        public string eposta { get; set; }

        [Display(Name = "Parolanızı Girin")]
        [Required(ErrorMessage = "Giriş Parolanızı Giriniz!")]
        [DataType(DataType.Password)]
        public string parola { get; set; }

        public string mesaj { get; set; }
    }
}