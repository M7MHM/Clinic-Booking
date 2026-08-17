using Clinic.Domain.Common;
using Clinic.Domain.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext dbContext)
        {

            string[] roles = { "Admin", "Doctor", "Patient" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }

            var adminEmail = "admin@clinic.com";

            var adminUser =
                await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Admin",
                    UserType = "Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(
                    newAdmin,
                    "Admin@123456");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(
                        newAdmin,
                        "Admin");
                }
            }
            var doctorCount = await dbContext.Doctors.CountAsync();

            if (doctorCount < 1000)
            {
                var doctors = new List<Doctor>();

                for (int i = doctorCount + 1; i <= 1000; i++)
                {
                    var doctor = new Doctor(
                        name: $"Doctor {i}",
                        age: 25 + (i % 40),
                        specialization:
                            i % 2 == 0
                                ? "Cardiology"
                                : "Dermatology",
                        email: $"doctor{i}@clinic.com"
                    );

                    doctors.Add(doctor);
                }

                await dbContext.Doctors.AddRangeAsync(doctors);
                await dbContext.SaveChangesAsync();
            }
            var patientCount = await dbContext.Patients.CountAsync();

            if (patientCount < 1000)
            {
                var patients = new List<Patient>();

                for (int i = patientCount + 1; i <= 1000; i++)
                {
                    var patient = new Patient(
                        name: $"patient {i}",
                        age: 25 + (i % 40),
                        email: $"patient{i}@clinic.com"
                    );

                    patients.Add(patient);
                }

                await dbContext.Patients.AddRangeAsync(patients);
                await dbContext.SaveChangesAsync();
            }
            var appointmentCount = await dbContext.Appointments.CountAsync();

            if (appointmentCount < 1000)
            {
                var doctorIds = await dbContext.Doctors.Select(d => d.Id).Take(100).ToListAsync();
                var patientIds = await dbContext.Patients.Select(p => p.Id).Take(100).ToListAsync();

                if (doctorIds.Any() && patientIds.Any())
                {
                    var appointments = new List<Domain.Tables.Appointment>();
                    var random = new Random();

                    for (int i = appointmentCount + 1; i <= 1000; i++)
                    {
                        var randomDoctorId = doctorIds[random.Next(doctorIds.Count)];
                        var randomPatientId = patientIds[random.Next(patientIds.Count)];

                        var appointment = new Domain.Tables.Appointment(
                            randomDoctorId,
                            randomPatientId,
                            $"Follow-up Visit {i}",
                            DateTime.UtcNow.AddDays(random.Next(1, 60)),
                            $"Regular checkup notes {i}"
                        );

                        appointments.Add(appointment);
                    }

                    await dbContext.Appointments.AddRangeAsync(appointments);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}