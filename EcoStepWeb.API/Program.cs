using EcoStepWeb.Business;
using EcoStepWeb.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddBusinessModule();
builder.Services.AddDataModule(builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", _ =>
    {
        _.AllowAnyOrigin();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.Services.SeedDevelopmentDataAsync();

    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.Servers = [];
    });
}

//app.UseHttpsRedirection();

app.UseCors("AllowAnyOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
