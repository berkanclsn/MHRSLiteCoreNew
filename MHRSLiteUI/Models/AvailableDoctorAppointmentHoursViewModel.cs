using MHRSLiteEntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI.Models
{
    public class AvailableDoctorAppointmentHoursViewModel
    {
        //dr ad
        //tarih --> yarın verilsin
        //saat başları
        //saat detayları
        public Doctor Doctor { get; set; }
        public int HospitalClinicId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string HourBase { get; set; }
        public List<string> Hours { get; set; } = new List<string>();
    }
}
