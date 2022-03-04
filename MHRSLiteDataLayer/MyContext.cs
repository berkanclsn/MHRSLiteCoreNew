using MHRSLiteEntityLayer.IdentityModels;
using MHRSLiteEntityLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteDataLayer
{
    public class MyContext:IdentityDbContext<AppUser,AppRole,string>
    {
        public MyContext(DbContextOptions<MyContext> options)
            :base(options)
        {

        }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Hospital> Hospitals { get; set; }
        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<HospitalClinic> HospitalClinics { get; set; }
        public virtual DbSet<AppointmentHour> AppointmentHours { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Deneme> Denemeler { get; set; }
        
    }
}
