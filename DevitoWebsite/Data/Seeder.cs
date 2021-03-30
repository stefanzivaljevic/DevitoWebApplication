using DevitoWebsite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevitoWebsite.Data
{
    public class Seeder
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public Seeder(ApplicationDbContext context, IWebHostEnvironment hosting, UserManager<StoreUser> userManager)
        {
            _context = context;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            StoreUser user = await _userManager.FindByEmailAsync("stefan@gmail.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Stefan",
                    LastName = "Zivaljevic",
                    Email = "stefan@gmail.com",
                    UserName = "stefan@gmail.com",
                    Address = "Saddasd Dasdas 4",
                    CountryId = 8,
                    PostalNumber = 11000
                };

                var result = await _userManager.CreateAsync(user, "Linux123!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Not created in seeder. ");
                }
            }

            if (!_context.Countries.Any())
            {

                var filepath = Path.Combine(_hosting.ContentRootPath, "Data/cont.json");
                var json = File.ReadAllText(filepath);
                var countries = JsonConvert.DeserializeObject<IEnumerable<Country>>(json);
                _context.Countries.AddRange(countries);

                
                _context.SaveChanges();
                

            }

            if (!_context.Products.Any())
            {
                _context.AddRange(

                            new Product()
                            {

                                Title = "Duks broj 1",
                                Description = "Test 1",
                                ItemNumber = "561424512",
                                Sizes = new List<Size>
                                {
                                        new Size() { SizeLetter = "XS" },
                                        new Size() { SizeLetter = "S" },

                                },
                                Price = 5500,
                                Image = "/lib/images/duks.png"
                            },
                            new Product()
                            {

                                Title = "Duks broj 2",
                                Description = "Test 2",
                                ItemNumber = "561424513",
                                Sizes = new List<Size>
                                {
                                        new Size() { SizeLetter = "XS" },
                                        new Size() { SizeLetter = "S" },
                                },
                                Price = 5500,
                                Image = "/lib/images/duks.png"
                            },
                            new Product()
                            {

                                Title = "Duks broj 3",
                                Description = "Test 3",
                                ItemNumber = "561424514",
                                Sizes = new List<Size>
                                {
                                    new Size() { SizeLetter = "XS" },
                                    new Size() { SizeLetter = "S" },
                                },
                                Price = 5500,
                                Image = "/lib/images/duks.png"
                            },
                            new Product()
                            {

                                Title = "Duks broj 4",
                                Description = "Test 4",
                                ItemNumber = "561424552",
                                Sizes = new List<Size>
                                {
                                    new Size() { SizeLetter = "XS" },
                                    new Size() { SizeLetter = "S" },
                                },
                                Price = 5500,
                                Image = "/lib/images/duks.png"
                            }



                    );

                _context.SaveChanges();
            }
        }
    }
}
