using EcoStepWeb.API.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

namespace EcoStepWeb.API;

public static class APIModule
{
    public static WebApplicationBuilder AddAPIModule(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<SchemeTransformer>();
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOrigin", _ =>
            {
                _.AllowAnyOrigin()
                 .AllowAnyHeader()
                 .AllowAnyMethod();
            });
        });

        var jwtKey = builder.Configuration["Jwt:Key"]!;

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwtOptions =>
            {
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };

                jwtOptions.MapInboundClaims = false;
            });

        var requireAuthPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        builder.Services.AddAuthorizationBuilder()
            .SetFallbackPolicy(requireAuthPolicy);

        return builder;
    }

    public static WebApplication MapOpenApiModule(this WebApplication app)
    {
        app.MapOpenApi().AllowAnonymous();

        app.MapScalarApiReference(options =>
        {
            options.Servers = [];
            options.AddPreferredSecuritySchemes(JwtBearerDefaults.AuthenticationScheme);
            options.EnablePersistentAuthentication();
        }).AllowAnonymous();

        return app;
    }
}
