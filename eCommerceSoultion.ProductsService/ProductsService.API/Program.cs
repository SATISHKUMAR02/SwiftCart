using BusinesslogicLayer;
using DataAccessLayer;
using FluentValidation.AspNetCore;
using ProductsService.API.Middlewares;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBusinesslogicLayer();
builder.Services.AddDataAccessLayer(builder.Configuration);// second parameter needs to passed seperately
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();



var app = builder.Build();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.UseExceptionHandlingMiddleware();
app.Run();
