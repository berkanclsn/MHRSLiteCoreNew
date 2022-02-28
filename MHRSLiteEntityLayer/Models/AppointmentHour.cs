using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer.Models
{
    [Table("AppointmentHours")]
    public class AppointmentHour:Base<int>
    {
        public int HospitalClinicId { get; set; }

        [Required]
        public string Hours { get; set; }

        [ForeignKey("HospitalClinicId")]
        public virtual HospitalClinic HospitalClinic { get; set; }
    }
}
