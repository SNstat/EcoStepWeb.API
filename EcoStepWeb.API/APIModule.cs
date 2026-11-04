using EcoStepWeb.API.Jwt;
using EcoStepWeb.API.OpenApi;
using EcoStepWeb.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Text.Json;

namespace EcoStepWeb.API;

public static class APIModule
{
    public static WebApplicationBuilder AddAPIModule(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>("DatabaseCheck")
            .AddApplicationLifecycleHealthCheck();

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

        var jwtKey = JwtKeyGenerator.GenerateSecureKey();
        builder.Configuration["Jwt:Key"] = jwtKey;

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

        app.MapHealthChecks("/health", CreateHealthCheckOptions()).AllowAnonymous();

        return app;
    }

    private static HealthCheckOptions CreateHealthCheckOptions()
    {
        return new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(e => new
                    {
                        name = e.Key,
                        status = e.Value.Status.ToString(),
                        duration = e.Value.Duration.TotalMilliseconds + "ms",
                        description = e.Value.Description
                    })
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        };
    }
}
