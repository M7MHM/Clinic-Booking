using System.Text;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.Identity.Interfaces;
using Clinic.Domain.Common;
using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using Clinic.Infrastructure.Data;
using Clinic.Infrastructure.Messaging;
using Clinic.Infrastructure.Repos;
using Clinic.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace Clinic.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IDoctorRepo, DoctorRepo>();
            services.AddScoped<IPatientRepo, PatientRepo>();
            services.AddScoped<IAppointmentRepo, AppointmentRepo>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            var jwtKey = configuration["Jwt:Key"] ?? "FallbackSecretKeyThatIsVeryLongAndSecureForLocalDev2026!";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            var redisConnection =
                configuration["Redis:ConnectionString"] ?? "localhost:6379";

            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnection));

            services.AddScoped<ICacheService, RedisCacheService>();

            services.AddScoped<IMessageProducer, RabbitMQProducer>();
            services.AddHostedService<NotificationConsumer>();
                services.AddSingleton<IConnectionMultiplexer>(
                    ConnectionMultiplexer.Connect(redisConnection));

                services.AddScoped<ICacheService, RedisCacheService>();

            return services;
        }
    }
}