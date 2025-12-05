using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AdminIvoire.WebApi.Authentication;

/// <summary>
/// Extensions pour l'enregistrement de l'authentification JWT
/// </summary>
public static class ServicesExtensions
{
    /// <summary>
    /// Configure l'authentification JWT pour l'application
    /// </summary>
    /// <param name="services">La collection de services</param>
    /// <param name="configuration">La configuration de l'application</param>
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = configuration["Jwt:Authority"];
            options.Audience = configuration["Jwt:Audience"];
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
    }
}
