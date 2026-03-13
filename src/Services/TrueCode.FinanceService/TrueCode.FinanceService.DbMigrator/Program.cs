using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrueCode.FinanceService.Infrastructure;
using TrueCode.FinanceService.Infrastructure.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDatabase(builder.Configuration);

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();

var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

try
{
    var pendingMigrations = (await db.Database.GetPendingMigrationsAsync()).ToArray();
    if (pendingMigrations.Length > 0)
    {
        var count = pendingMigrations.Length;
        var names = string.Join(", ", pendingMigrations);
        logger.LogInformation("Найдено {Count} ожидающих миграций: {Names}.", count, names);

        logger.LogInformation("Применяю миграции...");
        await db.Database.MigrateAsync();
        logger.LogInformation("Все миграции успешно применены!");
    }
    else
    {
        logger.LogInformation("Ожидающие миграции отсутствуют.");
    }
}
catch (Exception ex)
{
    logger.LogError(ex, "Произошла непредвиденная ошибка.");
}
