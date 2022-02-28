using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI.Components
{
    public class PatientAppointmentsViewComponent : ViewComponent
    {
        //Global alan
        private readonly IUnitOfWork _unitOfWork;

        //Dependency Injection
        public PatientAppointmentsViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IViewComponentResult Invoke()
        {
            PastAndFutureAppointmentsViewModel data = new PastAndFutureAppointmentsViewModel();
            DateTime today = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            data.UpcomingAppointments = _unitOfWork.AppointmentRepository
                .GetAll(x => 
                x.PatientId == HttpContext.User.Identity.Name && x.AppointmentDate > today ||
                (x.AppointmentDate == today && (Convert.ToInt32(x.AppointmentHour.Substring(0, 2)) > DateTime.Now.Hour || (Convert.ToInt32(x.AppointmentHour.Substring(0, 2)) == DateTime.Now.Hour && Convert.ToInt32(x.AppointmentHour.Substring(3, 2)) >= DateTime.Now.Minute))), includeProperties: "HospitalClinic").ToList();

            data.PastAppointments = _unitOfWork.AppointmentRepository
                .GetAll(x =>
                x.PatientId == HttpContext.User.Identity.Name && x.AppointmentDate <= today || 
                (x.AppointmentDate == today && (Convert.ToInt32(x.AppointmentHour.Substring(0, 2)) < DateTime.Now.Hour || (Convert.ToInt32(x.AppointmentHour.Substring(0, 2)) == DateTime.Now.Hour && Convert.ToInt32(x.AppointmentHour.Substring(3, 2)) < DateTime.Now.Minute))), includeProperties: "HospitalClinic").ToList();




            return View(data);
        }
    }
}
