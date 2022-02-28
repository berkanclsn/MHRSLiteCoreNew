using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer.Models
{
    public  abstract class PersonBase
    {
        [Key]
        [Column(Order = 1)]
        [MinLength(11)]
        [StringLength(11,ErrorMessage = "TC Kimlik numarası 11 haneli olmalıdır!")]
        public string TCNumber { get; set; }

    }
}
