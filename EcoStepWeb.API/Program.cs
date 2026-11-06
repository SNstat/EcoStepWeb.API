using EcoStepWeb.API.Authentication;
using EcoStepWeb.API.OpenApi;
using EcoStepWeb.Business;
using EcoStepWeb.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddBusinessModule();

builder.Services.AddDataModule(builder.Configuration["DatabasePath"]!);

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

var jwtKeyService = new JwtKeyService(builder.Configuration);
builder.Services.AddSingleton<IJwtKeyService>(jwtKeyService);

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKeyService.GetJwtKey()))
        };

        jwtOptions.MapInboundClaims = false;
    });

var requireAuthPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(requireAuthPolicy);

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions
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
}).AllowAnonymous();

await app.Services.SeedMockDataAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();

    app.MapScalarApiReference(options =>
    {
        options.Servers = [];
        options.AddPreferredSecuritySchemes(JwtBearerDefaults.AuthenticationScheme);
        options.EnablePersistentAuthentication();
    }).AllowAnonymous();
} else
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAnyOrigin");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
