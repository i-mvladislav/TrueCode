using Moq;
using TrueCode.FinanceService.Application.Currencies.Queries.GetCurrenciesByUser;
using TrueCode.FinanceService.Domain.Dao;
using TrueCode.FinanceService.Domain.Entities;
using TrueCode.UserService.HttpClients;

namespace TrueCode.FinanceService.Tests.Queries;

public class GetCurrenciesByUserQueryHandlerTests
{
    [Test]
    public async Task get_currencies_by_user_returns_error_on_empty_user()
    {
        // Arrange
        var clientMock = new Mock<ICurrenciesHttpClient>();
        clientMock.Setup(s => s.GetFavoriteCurrenciesAsync(
            It.Is<Guid>(userId => userId == Guid.Empty),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(["AUD"]);
        
        var currencyStorageMock = new Mock<ICurrencyStorage>();
        currencyStorageMock.Setup(s => s.GetCurrenciesByCodesAsync(
            It.IsAny<IEnumerable<string>>(),
            It.IsAny<CancellationToken>()));

        var query = new GetCurrenciesByUserQuery
        {
            UserId = Guid.Empty,
            JwtToken = "token",
        };
        var sut = new GetCurrenciesByUserQueryHandler(clientMock.Object, currencyStorageMock.Object);
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
            Assert.That(result.Errors[0].Message, Is.EqualTo("Некорректный пользователь."));
        });
    }

    [Test]
    public async Task get_currencies_by_user_returns_error_on_empty_token()
    {
        // Arrange
        var clientMock = new Mock<ICurrenciesHttpClient>();
        clientMock.Setup(s => s.GetFavoriteCurrenciesAsync(
                It.IsAny<Guid>(),
                It.Is<string>(s => string.IsNullOrWhiteSpace(s)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(["AUD"]);
        
        var currencyStorageMock = new Mock<ICurrencyStorage>();
        currencyStorageMock.Setup(s => s.GetCurrenciesByCodesAsync(
            It.IsAny<IEnumerable<string>>(),
            It.IsAny<CancellationToken>()));
        
        var query = new GetCurrenciesByUserQuery
        {
            UserId = Guid.NewGuid(),
            JwtToken = "",
        };
        var sut = new GetCurrenciesByUserQueryHandler(clientMock.Object, currencyStorageMock.Object);
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
            Assert.That(result.Errors[0].Message, Is.EqualTo("Некорректный токен."));
        });
    }

    [Test]
    public async Task get_currencies_by_user_returns_currencies()
    {
        // Arrange
        List<string> favouriteCurrencies = ["AUD"];
        List<CurrencyEntity> currencies =
        [
            new()
            {
                Id = "AUD",
                Name = "Австралийский доллар",
                Rate = 56.3353M
            }
        ];
        
        var clientMock = new Mock<ICurrenciesHttpClient>();
        clientMock.Setup(s => s.GetFavoriteCurrenciesAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(favouriteCurrencies);
        
        var currencyStorageMock = new Mock<ICurrencyStorage>();
        currencyStorageMock.Setup(s => s.GetCurrenciesByCodesAsync(
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(currencies);

        var query = new GetCurrenciesByUserQuery
        {
            UserId = Guid.NewGuid(),
            JwtToken = "token",
        };
        var sut = new GetCurrenciesByUserQueryHandler(clientMock.Object, currencyStorageMock.Object);
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Errors, Is.Empty);
            Assert.That(result.Data, Is.EquivalentTo(currencies));
        });
    }
}