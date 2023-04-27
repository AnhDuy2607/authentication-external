using POS.Model.Configuration;
using POS.Authentication;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.DataProtection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var googleAuthConfig = builder.Configuration.GetSection("Authentication:Google").Get<GoogleAuthenticationConfiguration>();
if (googleAuthConfig == null)
{
    throw new Exception("googleAuthConfig is null");
}

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

builder.Services.RegisterGoogleAuthentication(googleAuthConfig);
builder.Services.AddCors(_ =>
{
    _.AddPolicy("CorsPolicy", builder =>
        builder.WithOrigins("https://localhost:8080", "https://testauth.dichvubanker.com")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials()
    );
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
