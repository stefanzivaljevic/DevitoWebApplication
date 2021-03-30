using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using DevitoWebsite.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Vereyon.Web;

namespace DevitoWebsite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IConfiguration _config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<StoreUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.SignIn.RequireConfirmedEmail = true;
                cfg.Password.RequiredLength = 8;
                cfg.Password.RequireDigit = true;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddCookie()
                //.AddJwtBearer(cfg =>
                //{
                //    cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                //    {
                //        ValidIssuer = _config["Tokens:Issuer"],
                //        ValidAudience = _config["Tokens:Audience"],
                //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
                //    };
                //}
                //)
            ;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    _config.GetConnectionString("DefaultConnection")),ServiceLifetime.Scoped);

            services.AddFlashMessage();
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddMvc();

            services.AddControllers().AddNewtonsoftJson();
            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddRazorPages().AddNewtonsoftJson();

            services.AddTransient<Seeder>();

            //services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IDevitoRepository, DevitoRepository>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            //services.AddMvc()
            //    .AddJsonOptions(opt=>opt.JsonSerializerOptions.)
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                app.Use(async (ctx, next) =>
                {
                    await next();

                    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                    {
                        //Re-execute the request so the user gets the error page
                        string originalPath = ctx.Request.Path.Value;
                        ctx.Items["originalPath"] = originalPath;
                        ctx.Request.Path = "/Home/Error";
                        await next();
                    }
                });
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}