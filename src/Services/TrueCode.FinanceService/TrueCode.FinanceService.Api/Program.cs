using System.Text;
using TrueCode.FinanceService.Api;
using TrueCode.FinanceService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseApiServices();

await app.RunAsync();