using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer.IdentityModels
{
  public  class AppRole:IdentityRole
    {
        [StringLength(400,ErrorMessage ="Role açıklamasına en fazla 400 karakter girilebilir. ")]
        public string Description { get; set; }
    }
}
