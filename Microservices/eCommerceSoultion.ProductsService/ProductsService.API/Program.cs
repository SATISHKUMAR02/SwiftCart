using BusinesslogicLayer;
using BusinesslogicLayer.Validators;
using DataAccessLayer;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using ProductsService.API.Middlewares;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddBusinesslogicLayer();
builder.Services.AddDataAccessLayer(builder.Configuration);// second parameter needs to passed seperately
builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
}));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.UseExceptionHandlingMiddleware();
app.Run();
