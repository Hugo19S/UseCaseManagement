using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Text.Json.Serialization;
using UseCaseManagement.Application;
using UseCaseManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(
        x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

/******** Integrate KeyCloak Service ********/
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
     {
         options.RequireHttpsMetadata = false;
         options.Audience = builder.Configuration["Authentication:Audience"];
         options.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]!;
         options.TokenValidationParameters = new()
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateIssuerSigningKey = true,
             ValidIssuer = builder.Configuration["Authentication:ValidIssuer"],
             ValidAudience = builder.Configuration["Authentication:Audience"],
             RoleClaimType = builder.Configuration["Authentication:RoleAccess"]
         };
     });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();