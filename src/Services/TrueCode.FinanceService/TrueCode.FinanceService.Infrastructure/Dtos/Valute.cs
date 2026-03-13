using System.Globalization;
using System.Xml.Serialization;

namespace TrueCode.FinanceService.Infrastructure.Dtos;

public sealed record Valute
{
    [XmlAttribute("ID")]
    public required string Id { get; init; }
    [XmlElement("Name")]
    public required string Name { get; init; }
    [XmlElement("VunitRate")]
    public required string RateStr { get; init; }

    public decimal Rate => decimal.Parse(RateStr.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture);
}