
using BusinessLogicLayer;
using BusinessLogicLayer.HttpClients;
using BusinessLogicLayer.Policies;
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

builder.Services.AddTransient<IUsersMicroservicePolicies, UsersMicroservicePolicies>();
builder.Services.AddTransient<IProductMicroservicePolicies, ProductsMicroservicePolicies>();

// communicating with users microservice
builder.Services.AddHttpClient<UsersMicroserviceClient>(client =>
{

    client.BaseAddress = new Uri($"http://{builder.Configuration["UsersMircorserviceName"]}:{builder.Configuration["UsersMicroservicePort"]}");


}).AddPolicyHandler(builder.Services.BuildServiceProvider().GetRequiredService<IUsersMicroservicePolicies>().GetCombinedPolicy());
//    .AddPolicyHandler(builder.Services.BuildServiceProvider().GetRequiredService<IUsersMicroservicePolicies>().GetRetryPolicy())
//.AddPolicyHandler(builder.Services.BuildServiceProvider().GetRequiredService<IUsersMicroservicePolicies>().GetCircuitBreakerPolicy())
//.AddPolicyHandler(builder.Services.BuildServiceProvider().GetRequiredService<IUsersMicroservicePolicies>().GetTimeoutPolicy());





builder.Services.AddHttpClient<ProductMicroserviceClient>(client =>
{

    client.BaseAddress = new Uri($"http://{builder.Configuration["ProductsMircorserviceName"]}:{builder.Configuration["ProductsMicroservicePort"]}");


}).
AddPolicyHandler(builder.Services.BuildServiceProvider().GetRequiredService<IProductMicroservicePolicies>().GetCombinedPPolicy());


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
