using AutoMapper;
using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteDataLayer;
using MHRSLiteEntityLayer.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteBusinessLayer.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyContext _myContext;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public UnitOfWork(MyContext myContext)
        {
            _myContext = myContext;
            //UnitOfWork tüm repositoryleri oluşturacak.
            CityRepository = new CityRepository(_myContext);
            DistrictRepository = new DistrictRepository(_myContext);
            DoctorRepository = new DoctorRepository(_myContext);
            PatientRepository = new PatientRepository(_myContext);
            HospitalRepository = new HospitalRepository(_myContext);
            ClinicRepository = new ClinicRepository(_myContext);
            HospitalClinicRepository = new HospitalClinicRepository(_myContext);
            AppointmentRepository = new AppointmentRepository(_myContext, _mapper, _userManager);
            AppointmentHourRepository = new AppointmentHourRepository(_myContext);
        }

        public ICityRepository CityRepository { get; private set; }

        public IDistrictRepository DistrictRepository { get; private set; }

        public IDoctorRepository DoctorRepository { get; private set; }

        public IPatientRepository PatientRepository { get; private set; }

        public IHospitalRepository HospitalRepository { get; private set; }

        public IClinicRepository ClinicRepository { get; private set; }

        public IHospitalClinicRepository HospitalClinicRepository { get; private set; }

        public IAppointmentRepository AppointmentRepository { get; private set; }

        public IAppointmentHourRepository AppointmentHourRepository { get; private set; }

        public void Dispose()
        {
            _myContext.Dispose();
        }
    }
}
