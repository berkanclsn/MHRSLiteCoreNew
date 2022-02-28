using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteBusinessLayer.EmailService;
using MHRSLiteEntityLayer;
using MHRSLiteEntityLayer.Enums;
using MHRSLiteEntityLayer.IdentityModels;
using MHRSLiteEntityLayer.Models;
using MHRSLiteUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MHRSLiteUI.Controllers
{
    public class AccountController : Controller
    {
        //Global alan
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        //Dependency Injection
        public AccountController(
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
            CheckRoles();
        }

        private void CheckRoles()
        {
            var allRoles = Enum.GetNames(typeof(RoleNames));
            foreach (var item in allRoles)
            {
                if (!_roleManager.RoleExistsAsync(item).Result)
                {
                    var result = _roleManager.CreateAsync(new AppRole()
                    {
                        Name = item,
                        Description = item
                    }).Result;
                }
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var checkUserForUserName = await _userManager.FindByNameAsync(model.TCNumber);
                if (checkUserForUserName != null)
                {
                    ModelState.AddModelError(nameof(model.TCNumber), "Bu TCKimlik zaten sistemde kayıtlıdır.");
                    return View(model);
                }
                var checkUserForEmail = await _userManager.FindByEmailAsync(model.Email);
                if (checkUserForEmail != null)
                {
                    ModelState.AddModelError(nameof(model.Email), "Bu email zaten sistemde kayıtlıdır!");
                    return View(model);
                }
                // Yeni kullanıcı oluşturalım
                AppUser newUser = new AppUser()
                {
                    Email = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    UserName = model.TCNumber,
                    Gender = model.Gender
                    //TODO: Birthdate?
                    //TODO: Phone Number?
                };
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(newUser, RoleNames.Passive.ToString());

                    //email gönderilmelidir
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callBackUrl = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, code = code }, protocol: Request.Scheme);

                    var emailMessage = new EmailMessage()
                    {
                        Contacts = new string[] { newUser.Email },
                        Subject = "MHRSLITE - Email Aktivasyonu",
                        Body = $"Merhaba {newUser.Name} {newUser.Surname}, <br/>Hesabınızı aktifleştirmek için <a href='{HtmlEncoder.Default.Encode(callBackUrl)}'>buraya</a> tıklayınız."
                    };

                    await _emailSender.SendAsync(emailMessage);

                    //patient tablosuna ekleme yapılmalıdır.
                    Patient newPatient = new Patient()
                    {
                        TCNumber = model.TCNumber,
                        UserId = newUser.Id
                    };
                    if (_unitOfWork.PatientRepository.Add(newPatient) == false)
                    {
                        var emailMessageToAdmin = new EmailMessage()
                        {
                            Contacts = new string[]
                        { _configuration.GetSection("ManagerEmails:Email").Value },
                            CC = new string[] { _configuration.GetSection("ManagerEmails:EmailToCC").Value },
                            Subject = "MHRSLITE - HATA! Patient Tablosu",
                            Body = $"Aşağıdaki bilgilere sahip kişi eklenirken hata olmuş. Patient Tablosuna bilgileri ekleyiniz. <br/> Bilgiler: TcNumber:{model.TCNumber} <br/> UserId:{newUser.Id}"
                        };
                        await _emailSender.SendAsync(emailMessageToAdmin);
                    }
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Beklenmedik bir hata oluştu!");
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                return View();
            }

        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId,
            string code)
        {
            try
            {
                if (userId == null || code == null)
                {
                    return NotFound("Sayfa Bulunamadı!");
                }
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("Kullanıcı Bulunamadı!");
                }
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                //EmailConfirmed=1 ya da True
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    //User pasif rolde mi?
                    if (_userManager.IsInRoleAsync(user, RoleNames.Passive.ToString()).Result)
                    {
                        await _userManager.RemoveFromRoleAsync(user, RoleNames.Passive.ToString());
                        await _userManager.AddToRoleAsync(user, RoleNames.Patient.ToString());
                    }

                    TempData["EmailConfirmedMessage"] = "Hesabınız aktifleşmiştir...";
                    return RedirectToAction("Login", "Account");
                }

                //Login sayfasında bu tempdata view ekranında kontrol edilecektir.

                ViewBag.EmailConfirmedMessage = "Hesap aktifleştirme başarısızdır!";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.EmailConfirmedMessage = "Beklenmedik bir hata oldu! Tekrar deneyiniz.";

                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Veri girişleri düzgün olmalıdır!");
                    return View(model);
                }

                //user'ı bulup emailconfirmed kontrol edilsin.
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    //if (user.EmailConfirmed == false)
                    if (!user.EmailConfirmed)
                    {
                        //ViewBag.TheResult = "Sistemi kullanabilmeniz için üyeliğinizi aktifleştirmeniz gerekmektedir. Emailinize gönderilen aktivasyon linkine tıklayarak aktifleştirme işlemini yapabilirsiniz!";
                        ModelState.AddModelError("", "Sistemi kullanabilmeniz için üyeliğinizi aktifleştirmeniz gerekmektedir. Emailinize gönderilen aktivasyon linkine tıklayarak aktifleştirme işlemini yapabilirsiniz!");
                        return View(model);
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "TCKimlik veya şifre hatalıdır!");
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu!");
                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    ViewBag.ResetPasswordMessage = "Girdiğiniz email bulunamadı";
                }
                else
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callBackUrl = Url.Action("ConfirmResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Scheme);

                    var emailMessage = new EmailMessage()
                    {
                        Contacts=new string[] {user.Email },
                        Subject = "MHRSLITE - Şifremi unuttum",
                        Body = $"Merhaba {user.Name} {user.Surname}," +
                        $" <br/>Yeni parola belirlemek için" +
                        $" <a href='{HtmlEncoder.Default.Encode(callBackUrl)}'>buraya</a> tıklayınız. "
                    };
                    await _emailSender.SendAsync(emailMessage);
                    ViewBag.ResetPasswordMessage = "Emailinize şifre güncelleme yönergesi gönderilmiştir.";
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ResetPasswordMessage = "Beklenmedik bir hata oluştu!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult ConfirmResetPassword(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                // return BadRequest("deneme");
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı!");
                return View();
            }

            ViewBag.UserId = userId;
            ViewBag.Code = code;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı");
                    return View(model);
                }

                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));

                var result = await _userManager.ResetPasswordAsync(user, code, model.NewPassword);
                if (result.Succeeded)
                {
                    TempData["ConfirmResetPasswordMessage"] = "Şifreniz başarılı bir şekilde değiştirildi.";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "HATA: Şifreniz değiştirilemedi!");
                    return View(model);
                }


            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Beklenmedik hata oluştu!");
                return View(model);
            }
        }

    }

}
