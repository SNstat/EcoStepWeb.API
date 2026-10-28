using EcoStepWeb.Data.Entities;
using EcoStepWeb.Data.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace EcoStepWeb.Data.Seeds;

internal static class DataSeeder
{
    public static async Task SeedDevelopmentDataAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        var admin = new User
        {
            FirstName = "Ana",
            LastName = "Anić",
            UserName = "anaanic",
            PasswordHash = "AaaaAaaAaaAAAAa",
            RegistrationDate = DateTime.UtcNow,
            UserRole = UserRole.Admin
        };

        var user = new User
        {
            FirstName = "Pero",
            LastName = "Perić",
            UserName = "peroperic",
            PasswordHash = "BbbBbBBBbbBBBbbB",
            RegistrationDate = DateTime.UtcNow,
            UserRole = UserRole.User
        };

        context.Users.AddRange(admin, user);

        await context.SaveChangesAsync();

        var activity1 = new Activity
        {
            Title = "Čišćenje šume",
            Description = "Akcija čišćenja šume u Varaždinu.",
            Location = "Gradska šuma, Varaždin",
            StartingDateTime = DateTime.UtcNow.AddDays(14),
            EndingDateTime = DateTime.UtcNow.AddDays(14).AddHours(4),
            PointsForParticipation = 10,
            MaxParticipants = 50,
            OrganizerId = admin.Id
        };

        var activity2 = new Activity
        {
            Title = "Sadnja stabala u Kutini",
            Description = "Sadnja stabala u parku Kutine.",
            Location = "Gradski park, Kutina",
            StartingDateTime = DateTime.UtcNow.AddDays(10),
            EndingDateTime = DateTime.UtcNow.AddDays(10).AddHours(3),
            PointsForParticipation = 20,
            MaxParticipants = 45,
            OrganizerId = user.Id
        };

        context.Activities.AddRange(activity1, activity2);

        await context.SaveChangesAsync();
    }
}
