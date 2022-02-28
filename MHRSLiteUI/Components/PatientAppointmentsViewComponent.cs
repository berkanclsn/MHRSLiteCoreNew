using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteEntityLayer.PagingListModels;
using MHRSLiteEntityLayer.ViewModels;
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

        public IViewComponentResult Invoke(int pageNumberPast=1, int pageNumberFuture=1)
        {
            PastAndFutureAppointmentsViewModel data = new PastAndFutureAppointmentsViewModel();
            DateTime today = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            var patientId = HttpContext.User.Identity.Name;

            //Aktif randevular
            var upcomingappointments = _unitOfWork.AppointmentRepository.GetUpcomingAppointments(patientId);
            data.UpcomingAppointments = PaginatedList<AppointmentVM>.CreateAsync(upcomingappointments, pageNumberFuture, 4);

            //Geçmiş ve iptal randevular
            var pastAndCancelledappointments = _unitOfWork.AppointmentRepository.GetUpcomingAppointments(patientId);
            data.PastAppointments = PaginatedList<AppointmentVM>.CreateAsync(pastAndCancelledappointments, pageNumberPast, 4);


            //data.UpcomingAppointments = _unitOfWork.AppointmentRepository
            //    .GetAll(x => 
            //    x.PatientId == HttpContext.User.Identity.Name && x.AppointmentDate > today ||
            //    (x.AppointmentDate == today && (Convert.ToInt32(x.AppointmentHour.Substring(0, 2)) > DateTime.Now.Hour || (Convert.ToInt32(x.AppointmentHour.Substring(0, 2)) == DateTime.Now.Hour && Convert.ToInt32(x.AppointmentHour.Substring(3, 2)) >= DateTime.Now.Minute))), includeProperties: "HospitalClinic").ToList();

            //data.PastAppointments = _unitOfWork.AppointmentRepository
            //    .GetAll(x =>
            //    x.PatientId == HttpContext.User.Identity.Name && x.AppointmentDate <= today || 
            //    (x.AppointmentDate == today && (Convert.ToInt32(x.AppointmentHour.Substring(0, 2)) < DateTime.Now.Hour || (Convert.ToInt32(x.AppointmentHour.Substring(0, 2)) == DateTime.Now.Hour && Convert.ToInt32(x.AppointmentHour.Substring(3, 2)) < DateTime.Now.Minute))), includeProperties: "HospitalClinic").ToList();




            return View(data);
        }
    }
}
