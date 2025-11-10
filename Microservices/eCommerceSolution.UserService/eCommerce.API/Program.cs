using AutoMapper;

using eCommerce.Infrastructure;
using eCommerce.Core;
using eCommerce.API.Middlewares;
using System.Text.Json.Serialization;
using eCommerce.Core.Mapping;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCore();
// adding infrasturcture and core services in the main file so to use the services from their components here
builder.Services.AddInfrastructure();


// Correct AutoMapper registration in Program.cs
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());   
});

//builder.Services.AddAutoMapper(typeof(ApplicationUserMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ApplicationUserMappingProfile).Assembly);

// enabling fluent validations
//builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

//builder.Services.AddAutoMapper(typeof(ApplicationUserMappingProfile));
var app = builder.Build();

app.UseExceptionHandling();
// enable routing 
app.UseRouting();
// enable authentication
app.UseAuthorization();
app.UseSwagger(); // ===============================> to add swagger
app.UseSwaggerUI();
app.UseCors();
// enable authorization
app.UseAuthorization();
// enable map controllers
app.MapControllers();
app.Run();

