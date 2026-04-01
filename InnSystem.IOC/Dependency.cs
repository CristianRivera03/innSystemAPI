using InnSystem.DAL.DBConext;
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
        }
    }
}
