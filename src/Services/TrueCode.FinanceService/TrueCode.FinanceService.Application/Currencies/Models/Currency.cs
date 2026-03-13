namespace TrueCode.FinanceService.Application.Currencies.Models;

public sealed record Currency
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Rate { get; set; }
}