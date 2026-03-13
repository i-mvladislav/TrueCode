using Microsoft.EntityFrameworkCore;
using Quartz;
using TrueCode.FinanceService.Domain.Entities;
using TrueCode.FinanceService.Infrastructure.Data;
using TrueCode.FinanceService.Infrastructure.HttpClients;

namespace TrueCode.FinanceService.Infrastructure.Jobs;

public class CloneValutesJob(ICbrHttpClient client, ApplicationContext db) : IJob
{
    public static readonly JobKey Key = new(nameof(CloneValutesJob));
    
    public async Task Execute(IJobExecutionContext context)
    {
        var response = await client.GetCurrenciesAsync(context.CancellationToken);
        var entities = response.Valutes
            .Select(v => new CurrencyEntity
            {
                Id = v.Id,
                Name = v.Name,
                Rate = v.Rate,
            }).ToArray();

        await using (var transaction = await db.Database.BeginTransactionAsync(context.CancellationToken))
        {
            try
            {
                await db.Currencies.ExecuteDeleteAsync(context.CancellationToken);
            
                await db.Currencies.AddRangeAsync(entities, context.CancellationToken);
                await db.SaveChangesAsync(context.CancellationToken);
                
                await transaction.CommitAsync(context.CancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(context.CancellationToken);
                throw;
            }
        }
    }
}