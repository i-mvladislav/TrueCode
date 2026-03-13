using TrueCode.Core.Entities;

namespace TrueCode.FinanceService.Domain.Entities;

public class CurrencyEntity : Entity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Rate { get; set; }
}