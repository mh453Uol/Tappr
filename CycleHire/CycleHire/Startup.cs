using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CycleHire.Persistence;
using CycleHire.Services;
using CycleHire.Core.Repositories;
using CycleHire.Persistence.UnitOfWork;
using CycleHire.Core.Models;
using AutoMapper;
using Stripe;
using CycleHire.Utilites.Configuration;
using FluentEmail.Mailgun;
using FluentEmail.Core;

namespace CycleHire
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            // Register user secrets
            services.Configure<StripeSettingAuth>(Configuration.GetSection("PaymentSettings"));
            services.Configure<EmailSettingAuth>(Configuration.GetSection("EmailSettings"));
            services.Configure<AuthImageUploaderOptions>(Configuration.GetSection("ImageSettings"));

            // Add Stripe (Payment Gateway)
            // Pull secret key from secrets.json
            StripeConfiguration.SetApiKey(Configuration.GetSection("PaymentSettings")["SecretKey"]);


            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IListingRepository, ListingRepository>();
            services.AddScoped<IAccessoryRepository, AccessoryRepository>();
            services.AddScoped<IAvailabilityRepository, AvailabiltyRepository>();
            services.AddScoped<IHostRepository, HostRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IRoutePlannerRepository, RoutePlannerRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Seeding database
            services.AddTransient<UserRoleSeed>();
            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IImageUploader, ImageUploader>();
            services.AddTransient<IStripeService, Services.StripeService>();

            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserRoleSeed seed)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            seed.SeedAsync();
        }
    }
}
