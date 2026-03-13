namespace TrueCode.FinanceService.Api.ResponseDtos;

public sealed record Currency
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Rate { get; set; }
}