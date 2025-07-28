using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            context.Database.EnsureCreated();

            string[] roleNames = { "Admin", "Teacher", "Student" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // -------------------- Admin --------------------
            var adminConfig = configuration.GetSection("SeedUsers:Admin");
            var adminEmail = adminConfig["Email"];
            var adminPassword = adminConfig["Password"];

            if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
            {
                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var admin = new Admin
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FirstName = adminConfig["FirstName"],
                        LastName = adminConfig["LastName"],
                        AdminRole = adminConfig["AdminRole"],
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(admin, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                        Console.WriteLine("Admin seeded.");
                    }
                }
            }
            else
            {
                Console.WriteLine("⚠️  Admin email or password missing in appsettings.json");
            }

            // -------------------- Teacher --------------------
            var teacherConfig = configuration.GetSection("SeedUsers:Teacher");
            var teacherEmail = teacherConfig["Email"];
            var teacherPassword = teacherConfig["Password"];

            if (!string.IsNullOrWhiteSpace(teacherEmail) && !string.IsNullOrWhiteSpace(teacherPassword))
            {
                if (await userManager.FindByEmailAsync(teacherEmail) == null)
                {
                    var teacher = new Teacher
                    {
                        UserName = teacherEmail,
                        Email = teacherEmail,
                        FirstName = teacherConfig["FirstName"],
                        LastName = teacherConfig["LastName"],
                        Qualification = teacherConfig["Qualification"],
                        Specialization = teacherConfig["Specialization"],
                        JoinDate = DateTime.Now.AddYears(-2),
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(teacher, teacherPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(teacher, "Teacher");
                        Console.WriteLine("Teacher seeded.");
                    }
                }
            }
            else
            {
                Console.WriteLine("⚠️  Teacher email or password missing in appsettings.json");
            }

            // -------------------- Student --------------------
            var studentConfig = configuration.GetSection("SeedUsers:Student");
            var studentEmail = studentConfig["Email"];
            var studentPassword = studentConfig["Password"];

            if (!string.IsNullOrWhiteSpace(studentEmail) && !string.IsNullOrWhiteSpace(studentPassword))
            {
                if (await userManager.FindByEmailAsync(studentEmail) == null)
                {
                    var student = new Student
                    {
                        UserName = studentEmail,
                        Email = studentEmail,
                        FirstName = studentConfig["FirstName"],
                        LastName = studentConfig["LastName"],
                        Grade = studentConfig["Grade"],
                        ParentName = studentConfig["ParentName"],
                        ParentContact = studentConfig["ParentContact"],
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(student, studentPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(student, "Student");
                        Console.WriteLine("Student seeded.");
                    }
                }
            }
            else
            {
                Console.WriteLine("⚠️  Student email or password missing in appsettings.json");
            }
        }
    }
}
