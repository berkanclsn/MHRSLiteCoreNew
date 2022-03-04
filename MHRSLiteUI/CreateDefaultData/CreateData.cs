using ClosedXML.Excel;
using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteEntityLayer.Enums;
using MHRSLiteEntityLayer.IdentityModels;
using MHRSLiteEntityLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI.CreateDefaultData
{
    public static class CreateData
    {
        public static void Create(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment environment)
        {
            //Eklenmesini istediğim verileri ekleyecek metotları çağıralım...
            CheckRoles(roleManager);
            CreateCities(environment, unitOfWork);
        }
        private static void CheckRoles(RoleManager<AppRole> roleManager)
            {
            
                var allRoles = Enum.GetNames(typeof(RoleNames));
                foreach (var item in allRoles)
                {
                    if (!roleManager.RoleExistsAsync(item).Result)
                    {
                        var result = roleManager.CreateAsync(new AppRole()
                        {
                            Name = item,
                            Description = item
                        }).Result;
                    }
                }
            }

        private static void CreateCities(IWebHostEnvironment environment, IUnitOfWork unitOfWork)
        {
            try
            {
                //Provide a path for excel file
                string path = Path.Combine(environment.WebRootPath, "Excels");
                string fileName = Path.GetFileName("Cities.xlsx");
                string filePath = Path.Combine(path, fileName);
                using (var excelBook=new XLWorkbook(filePath))
                {
                    var rows = excelBook.Worksheet(1).RowsUsed();
                    foreach (var item in rows)
                    {
                        if (item.RowNumber()>1 && item.RowNumber()<=rows.Count())
                        {
                            var cell = item.Cell(2).Value; //İstanbul
                            City city = new City()
                            {
                                CreatedDate = DateTime.Now,
                                CityName = cell.ToString(),
                                PlateCode = Convert.ToByte(item.Cell(3).Value)
                            };
                            unitOfWork.CityRepository.Add(city);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        
    }
}
