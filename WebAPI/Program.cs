using System.Globalization;
using Application;
using Infrastructure;
using Infrastructure.SignalR;
using Microsoft.AspNetCore.OData;
using Scalar.AspNetCore;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddOpenApi();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebApi();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddOData(conf => { conf.EnableQueryFeatures(); });
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddSignalR();

var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var myAllowSpecificOrigins = "AllowAngular";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy  =>
        {
            policy
                .WithOrigins("http://localhost:4200") // Angular portunu ekle
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});
var app = builder.Build();
app.UseRouting();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseStaticFiles();
app.MapSwagger();
app.MapScalarApiReference();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapHub<NotificationHub>("/hub/notifications");

app.Run();