using DevIO.API.Data;
using DevIO.API.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevIO.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(connection));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddDefaultTokenProviders();

            return services;
        }
    }

}
