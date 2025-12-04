
using BusinessLogicLayer;
using BusinessLogicLayer.HttpClients;
using DataAccessLayer;
using FluentValidation.AspNetCore;
using OrdersMicroservice.API.Middlewares;
using Polly;

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


}).AddPolicyHandler(
    Policy.HandleResult<HttpResponseMessage>(r => r.!IsSuccessStatusCode).WaitAndRetry(retryCount: 5, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(2)));



builder.Services.AddHttpClient<ProductMicroserviceClient>(client =>
{

    client.BaseAddress = new Uri($"http://{builder.Configuration["ProductsMircorserviceName"]}:{builder.Configuration["ProductsMicroservicePort"]}");


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
