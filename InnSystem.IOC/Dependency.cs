using InnSystem.BLL.Services;
using InnSystem.BLL.Services.Contract;
using InnSystem.DAL.DBConext;
using InnSystem.DAL.Repositories;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.Model;
using InnSystem.Utility;
using InnSystem.Utility.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnSystem.IOC
{
    public static class Dependency
    {
        public static void DependencyInyections(this IServiceCollection services , IConfiguration configuration)
        {
            //llama al context de dal
            services.AddDbContext<InnDbContext>(options =>
            {
                //aca llama la conexion de la api en app setting 
                options.UseNpgsql(configuration.GetConnectionString("connectionDB"));
            });

            //Dependecnecia de repositorios
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();


            //automapper 
            services.AddAutoMapper(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            }, typeof(AutoMapperProfile));

            //Cloudinary 
            services.AddScoped<ICloudinaryUtility, CloudinaryUtility>();
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));



            // Email and PDF
            services.Configure<InnSystem.Utility.Settings.EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailService, InnSystem.Utility.Services.EmailService>();
            services.AddScoped<IPdfService, InnSystem.Utility.Services.PdfService>();

            // QuestPDF Community License
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            //Servicios
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IRoomTypeService, RoomTypeService>();
            services.AddScoped<IServiceManagerService, ServiceManagerService>();
            services.AddScoped<ISeasonService, SeasonService>();
            services.AddScoped<IPaymentService, PaymentService>();

        }
    }
}
