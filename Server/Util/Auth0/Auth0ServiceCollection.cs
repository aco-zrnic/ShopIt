using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Server.Util.Auth0
{
    public static class Auth0ServiceCollection
    {
        public static IServiceCollection AddAuthServiceCollection(this IServiceCollection services,
           IConfiguration configuration)
        {


            var authentication = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"https://{configuration.GetValue<string>("Auth0:Domain")}/";
                options.Audience = configuration.GetValue<string>("Auth0:Audience");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = configuration.GetValue<string>("Auth0:Audience"),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    RequireSignedTokens = true
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CreateAccess", policy =>
                                  policy.RequireClaim("permissions", "create:term"));
                options.AddPolicy("UpdateAccess", policy =>
                                  policy.RequireClaim("permissions", "update:term"));
                options.AddPolicy("ReadAccess", policy =>
                                  policy.RequireClaim("permissions", "read:term"));
                options.AddPolicy("DeleteAccess", policy =>
                                  policy.RequireClaim("permissions", "delete:term"));
            });



            return services;
        }
    }
}
