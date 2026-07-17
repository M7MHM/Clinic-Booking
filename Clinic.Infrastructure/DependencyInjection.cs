using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using Clinic.Infrastructure.Data;
using Clinic.Infrastructure.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IDoctorRepo, DoctorRepo>();
            services.AddScoped<IPatientRepo, PatientRepo>();
            services.AddScoped<IAppointmentRepo, AppointmentRepo>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
