using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteEntityLayer.Enums;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI.QuartzWork
{
    [DisallowConcurrentExecution]
    public class RomatologyClaimJob : IJob
    {
        private readonly ILogger<RomatologyClaimJob> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public RomatologyClaimJob(ILogger<RomatologyClaimJob> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var date = DateTime.Now.AddMonths(-1);
                //son bir aydaki dahiliyedeki iptal olan hariç tüm randevuşarı getir
                var appointment = _unitOfWork.AppointmentRepository.GetAppointmentsIM(date).OrderByDescending(x=>x.AppointmentDate).ToList();
                foreach (var item in appointment)
                {
                    //usera ait dahiliyeRomatoloji claimi yoksa eklenmeli
                    // yarın devam...
                    //varsa tarihi aynı mı? değilse sil ve yeniden ekle.
                    //claim yoksa claim ekle
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
