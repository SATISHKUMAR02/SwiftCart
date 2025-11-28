
using BusinessLogicLayer;
using BusinessLogicLayer.HttpClients;
using DataAccessLayer;
using FluentValidation.AspNetCore;
using OrdersMicroservice.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();



builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// communicating with users microservice
builder.Services.AddHttpClient<UsersMicroserviceClient>(client =>
{

    client.BaseAddress = new Uri($"http://{builder.Configuration["UsersMircorserviceName"]}:{builder.Configuration["UsersMicroservicePort"]}");


});


var app = builder.Build();
app.UseExceptionHandlingMiddleware();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
