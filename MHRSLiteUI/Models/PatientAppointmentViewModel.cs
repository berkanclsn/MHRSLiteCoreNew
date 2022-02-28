using MHRSLiteEntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI.Models
{
    public class PatientAppointmentViewModel
    {
        public int HospitalClinicId { get; set; }
        public int HospitalId { get; set; }
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int DistrictId { get; set; }
        public byte CityId { get; set; }
        public int ClinicId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AvailableHour { get; set; }

    }
}
