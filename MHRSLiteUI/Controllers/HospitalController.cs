using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteBusinessLayer.EmailService;
using MHRSLiteEntityLayer.IdentityModels;
using MHRSLiteEntityLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI.Controllers
{
    public class HospitalController : Controller
    {
        //Global alan
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        //Dependency Injection
        public HospitalController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }


        public JsonResult GetHospitalFromClinicId(int id, int districtid)
        {
            try
            {
                var data = new List<Hospital>();
                if (id > 0 && districtid > 0)
                {
                    data = _unitOfWork.HospitalClinicRepository
                        .GetAll(x => x.ClinicId == id)
                        .Select(y =>
                        y.Hospital)
                        .Where(x => x.DistrictId == districtid)
                        .Distinct().ToList();
                }
                return Json(new { isSuccess = true, data });

            }
            catch (Exception)
            {

                return Json(new { isSuccess = false });

            }
        }
    }
}
