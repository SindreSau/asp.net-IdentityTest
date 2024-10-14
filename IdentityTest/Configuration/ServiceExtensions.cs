using IdentityTest.Data;
using IdentityTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityTest.Configuration;

public static class ServiceExtensions
{
    public static void AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public static void AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Add database context
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"))
        );

        // Add Identity
        services.AddIdentityApiEndpoints<ApplicationUser>(options =>
            {
                // Configure identity requirements

                // Password requirements
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.@+";

                // Sign in settings
                options.SignIn.RequireConfirmedEmail = false; // TODO: Set to true in production

                // Disable 2FA
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                // Disable 2FA tokens
                options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultProvider;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        // Add authorization policies
        services.AddAuthorizationBuilder()
            .AddPolicy("WriterPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole("Writer");
            })
            .AddPolicy("AdminPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole("Admin");
            })
            .AddPolicy("EditOwnJokePolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole("Writer").RequireClaim("UserId");
            });
    }
}
