using TrueCode.UserService.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseApiServices();

await app.RunAsync();