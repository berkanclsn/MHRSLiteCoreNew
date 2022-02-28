using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteBusinessLayer.EmailService;
using MHRSLiteEntityLayer.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI.Controllers
{
    public class CityController: Controller
    {
        //Global alan
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        //Dependency Injection
        public CityController(
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

        public JsonResult GetCityDistricts(int id)
        {
            try
            {
                var data = _unitOfWork.DistrictRepository
                    .GetAll(x => x.CityId == id, orderBy: x => x.OrderBy(y => y.DistrictName));
                return Json(new { isSuccess = true, data });
            }
            catch (Exception ex)
            {

                return Json(new { isSuccess = false });
            }
        }
    }
}
