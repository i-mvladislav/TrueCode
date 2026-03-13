using TrueCode.Core.Commands;
using TrueCode.Core.Models;
using TrueCode.UserService.Domain.Dao;
using TrueCode.UserService.Domain.Entities;

namespace TrueCode.UserService.Application.Currencies.Commands.AddFavoriteCurrency;

public sealed class AddFavoriteCurrencyCommandHandler(ICurrencyStorage currencyStorage) : BaseCommandHandler<AddFavoriteCurrencyCommand>
{
    protected override async Task<CommandResult> ExecuteCoreAsync(AddFavoriteCurrencyCommand request, CancellationToken cancellationToken = default)
    {
        List<Error> errors = [];

        if (request.UserId == Guid.Empty)
            errors.Add(new Error("Некорректный пользователь."));
        
        if (string.IsNullOrWhiteSpace(request.CurrencyCode))
            errors.Add(new Error("Код пустой."));

        if (errors.Count > 0)
            return CommandResult.Failure(errors);

        var entity = new FavoriteCurrencyEntity
        {
            UserId = request.UserId,
            Code = request.CurrencyCode,
        };

        await currencyStorage.AddFavoriteCurrencyAsync(entity, cancellationToken);
        
        return CommandResult.Success();
    }
}