using InnSystem.DAL.DBConext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

using InnSystem.DAL.Repositories.Contract;
using InnSystem.DAL.Repositories;
using InnSystem.BLL.Services.Contract;
using InnSystem.BLL.Services;
using InnSystem.Utility;

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


            //Servicios
            services.AddScoped<IBookingService, BookingService>();

        }
    }
}
