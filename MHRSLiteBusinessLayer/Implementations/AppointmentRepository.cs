using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteDataLayer;
using MHRSLiteEntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteBusinessLayer.Implementations
{
    public class AppointmentRepository : Repository<Appointment>,
          IAppointmentRepository
    {
        public AppointmentRepository(MyContext myContext):base(myContext)
        {
           
        }
    }
}
