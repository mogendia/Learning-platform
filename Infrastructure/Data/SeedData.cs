using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Starting database seeding...");

            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Console.WriteLine("Applying migrations to ensure database is up to date...");
            await context.Database.MigrateAsync();
            Console.WriteLine("Migrations applied successfully.");

            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);

            Console.WriteLine("Database seeding finished successfully.");
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            Console.WriteLine("Seeding roles...");
            string[] roleNames = { "Student", "Instructor" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    Console.WriteLine($"Creating role: {roleName}");
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
                else
                {
                    Console.WriteLine($"Role '{roleName}' already exists. Skipping creation.");
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            Console.WriteLine("Seeding users...");

            // Seed Instructor User
            const string instructorEmail = "mehab1955@gmail.com";
            if (await userManager.FindByEmailAsync(instructorEmail) == null)
            {
                Console.WriteLine($"Creating instructor user: {instructorEmail}");
                var instructor = new ApplicationUser
                {
                    UserName = "Mostafa Ehab",
                    Email = instructorEmail,
                    FullName = "Mostafa Ehab",
                    EmailConfirmed = true, // Automatically confirm email for seeded users
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(instructor, "98991810Mo$");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(instructor, "Instructor");
                    Console.WriteLine("Instructor user created and assigned to 'Instructor' role.");
                }
                else
                {
                    // Log errors if creation fails
                    Console.WriteLine($"Failed to create instructor user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine($"User '{instructorEmail}' already exists. Skipping creation.");
            }

            // Seed Student User
            const string studentEmail = "muhammadgendia@gmail.com";
            if (await userManager.FindByEmailAsync(studentEmail) == null)
            {
                Console.WriteLine($"Creating student user: {studentEmail}");
                var student = new ApplicationUser
                {
                    UserName = "Muhammad Gendia",
                    Email = studentEmail,
                    FullName = "Muhammad Gendia",
                    EmailConfirmed = true, // Automatically confirm email for seeded users
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(student, "Omda123.");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(student, "Student");
                    Console.WriteLine("Student user created and assigned to 'Student' role.");
                }
                else
                {
                    // Log errors if creation fails
                    Console.WriteLine($"Failed to create student user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine($"User '{studentEmail}' already exists. Skipping creation.");
            }
        }
    }
}

