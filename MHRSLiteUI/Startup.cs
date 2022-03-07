using AutoMapper;
using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteBusinessLayer.EmailService;
using MHRSLiteBusinessLayer.Implementations;
using MHRSLiteDataLayer;
using MHRSLiteEntityLayer.Enums;
using MHRSLiteEntityLayer.IdentityModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MHRSLiteEntityLayer.Mappings;

namespace MHRSLiteUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Aspnet Core'un Connection String baðlantýsý yapabilmesi için 
            //servislerine dbcontext eklenmesi gerekir.
            services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
            });

            //********************************
            //IUnitOfWork gördüðün zaman bana UnitOfWork nesnesi üret!
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //IEmailSender gördüðün zaman bana EmailSender nesnesi üret!
            services.AddScoped<IEmailSender, EmailSender>();
            //IClaimsTransformation gördüðü zaman bizim yazdýðý classý üretecek!
            services.AddScoped<IClaimsTransformation, ClaimProvider.ClaimProvider>();
            //********************************
            services.AddAutoMapper(typeof(Maps));

            services.AddAuthorization(opts=>
            {
                opts.AddPolicy("GenderPolicy", policy =>
                 policy.RequireClaim("gender", Genders.Bayan.ToString())
                ); 
            });


            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation(); //Çalýþýrken razor sayfasýnda yapýlan deðiþikliklerin sayfaya yansýmasý için eklendi. 
            services.AddRazorPages();
            services.AddMvc();
            services.AddSession(options =>
             {
                 options.IdleTimeout = TimeSpan.FromSeconds(60);
             });
            //Google api'den alýnacak clientId ve clientSecret burada projeye dahil edilecek(secret.json'a ekleyip buraya eklemen lazým)
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = Configuration[""];
                options.ClientSecret = Configuration[""];
            });
            //**********************
            services.AddIdentity<AppUser, AppRole>(opts =>
             {
                 opts.User.RequireUniqueEmail = true;
                 opts.Password.RequiredLength = 6;
                 opts.Password.RequireNonAlphanumeric = false;
                 opts.Password.RequireLowercase = false;
                 opts.Password.RequireUppercase = false;
                 opts.Password.RequireDigit = false;
                 opts.User.AllowedUserNameCharacters =
"abcdefgðhijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
             }).AddDefaultTokenProviders().AddEntityFrameworkStores<MyContext>();

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IUnitOfWork unitOfWork)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();//wwwroot klasörünün kullanýlabilmesi için
            app.UseRouting(); //rooting mekanizmasý için
            app.UseSession(); // session oturum mekanizmasý için
            app.UseAuthentication(); //login logout kullanabilmek için
            app.UseAuthorization(); //authorization attiribute kullanabilmek için

            //Sabit verilerimiz eklenmesi için static metodu çaðýralým
            CreateDefaultData.CreateData.Create(userManager,roleManager,unitOfWork,Configuration,env);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    "management",
                    "management",
                    "management/{controller=Admin}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
