using MHRSLiteBusinessLayer.Contracts;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MHRSLiteEntityLayer.Enums;

namespace MHRSLiteUI.QuartzWork
{
    [DisallowConcurrentExecution]
    public class AppointmentStatusJob : IJob
    {
        private readonly ILogger<AppointmentStatusJob> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public AppointmentStatusJob(ILogger<AppointmentStatusJob> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                //Appointment tablosundaki aktif randevuların hepsini getirsin
                //Tarihi geçmiş olanları past statusüne çeksin
                var appointments = _unitOfWork.AppointmentRepository.GetAll(x => x.AppointmentStatus == AppointmentStatus.Active);
                foreach (var item in appointments)
                {
                    int itemHour = Convert.ToInt32(item.AppointmentHour.Substring(0, 2));
                    int itemMinute = Convert.ToInt32(item.AppointmentHour.Substring(3, 2));

                    if (item.AppointmentDate < DateTime.Now
                        ||
                        (item.AppointmentDate.ToShortDateString() ==
                            DateTime.Now.ToShortDateString()
                            &&
                            (itemHour < DateTime.Now.Hour || (itemHour == DateTime.Now.Hour && itemMinute < DateTime.Now.Minute))))
                    {
                        item.AppointmentStatus = AppointmentStatus.Past;
                        _unitOfWork.AppointmentRepository.Update(item);
                    }

                }
                _logger.LogInformation("AppointmentStatus updated");
                return Task.CompletedTask;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
