using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RunGroop.Data.Models.Identity;

namespace RunGroop.Data.Data
{
    public class UsersSeed
    {
        public static async Task SeedUsersAsync(IApplicationBuilder applicationBuilder, string adminUserEmail, string role)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                await userManager.AddToRoleAsync(adminUser, role);
            }

        }
    }
}

