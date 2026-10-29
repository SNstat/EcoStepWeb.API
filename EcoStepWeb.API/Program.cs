using EcoStepWeb.API;
using EcoStepWeb.Business;
using EcoStepWeb.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.AddAPIModule();
builder.Services.AddBusinessModule();
builder.Services.AddDataModule(builder.Configuration.GetConnectionString("DefaultConnection")!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.Services.SeedDevelopmentDataAsync();

    app.MapOpenApiModule();
}

//app.UseHttpsRedirection();

app.UseCors("AllowAnyOrigin");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
