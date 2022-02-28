using MHRSLiteEntityLayer.Enums;
using MHRSLiteEntityLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer.ViewModels
{
        public class AppointmentVM
        {
            public int Id { get; set; }
            public DateTime CreatedDate { get; set; }

            [Required]
            public string PatientId { get; set; }

            public int HospitalClinicId { get; set; }

            [Required]
            public DateTime AppointmentDate { get; set; }

            [Required]
            public string AppointmentHour { get; set; } //10:00

            public AppointmentStatus AppointmentStatus { get; set; }

            public Patient Patient { get; set; }
            public HospitalClinic HospitalClinic { get; set; }
        }

    }

