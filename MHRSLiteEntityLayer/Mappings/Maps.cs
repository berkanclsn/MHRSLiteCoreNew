using AutoMapper;
using MHRSLiteEntityLayer.Models;
using MHRSLiteEntityLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer.Mappings
{
    public class Maps:Profile
    {
        public Maps()
        {
            ////Appointment'ı AppointmentVM'ye dönüştür
            //CreateMap<Appointment, AppointmentVM>();
            ////AppointmentVM'i Appointment'a dönüştür
            //CreateMap<AppointmentVM, Appointment>();

            //ReverseMap sayesinde yukarıdaki 2 ayrı işlemi tek satırda yapmış oluyoruz.
            //Appointment AppointmentVM'e dönüşebilir
            //AppointmentVM Appointment'a dönüşebilir
            CreateMap<AppointmentVM, Appointment>().ReverseMap();
        }
    }
}
