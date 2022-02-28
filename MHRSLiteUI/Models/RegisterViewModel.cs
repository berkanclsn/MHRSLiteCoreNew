using MHRSLiteEntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "TC Kimlik Numarası")]
        [MinLength(11)]
        [StringLength(11, ErrorMessage = "TC Kimlik numarası 11 haneli olmalıdır!")]
        public string TCNumber { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Ad alanı gereklidir.")]
        [Display(Name = "Ad")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Soyad alanı gereklidir.")]
        [Display(Name = "Soyad")]
        [StringLength(50)]
        public string Surname { get; set; }
        [Required(ErrorMessage = "E-Posta alanı gereklidir.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre alanı gereklidir.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifreniz minimum 6 karaterli olmalıdır!")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
      
        [Required(ErrorMessage = "Cinsiyet bilgisi gereklidir!")]
        public Genders Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihiniz")]

        public DateTime BirthDate { get; set; }

    }
}
