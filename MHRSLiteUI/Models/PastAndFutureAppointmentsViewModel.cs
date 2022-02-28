using MHRSLiteEntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI.Models
{
    public class PastAndFutureAppointmentsViewModel
    {
        public List<Appointment> PastAppointments { get; set; }
        public List<Appointment> UpcomingAppointments { get; set; }
    }
}
