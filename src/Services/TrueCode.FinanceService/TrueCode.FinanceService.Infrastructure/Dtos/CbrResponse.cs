using System.Xml.Serialization;

namespace TrueCode.FinanceService.Infrastructure.Dtos;

[XmlRoot("ValCurs")]
public sealed class CbrResponse
{
    [XmlElement("Valute")]
    public List<Valute> Valutes { get; set; } = [];
}