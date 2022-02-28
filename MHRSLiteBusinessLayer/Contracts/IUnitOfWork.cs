using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteBusinessLayer.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        ICityRepository CityRepository { get; }

        IDistrictRepository DistrictRepository { get; }

        IDoctorRepository DoctorRepository { get; }

        IPatientRepository PatientRepository { get; }

        IHospitalRepository HospitalRepository { get; }

        IClinicRepository ClinicRepository { get; }

        IHospitalClinicRepository HospitalClinicRepository { get; }
        IAppointmentRepository AppointmentRepository { get; }
        IAppointmentHourRepository AppointmentHourRepository { get; }
    }
}
