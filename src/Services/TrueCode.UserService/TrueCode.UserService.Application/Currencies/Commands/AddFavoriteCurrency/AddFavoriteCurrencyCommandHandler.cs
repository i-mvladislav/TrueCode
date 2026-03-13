using TrueCode.Core.Commands;
using TrueCode.Core.Models;
using TrueCode.Core.Users;
using TrueCode.UserService.Domain.Dao;
using TrueCode.UserService.Domain.Entities;

namespace TrueCode.UserService.Application.Currencies.Commands.AddFavoriteCurrency;

internal sealed class AddFavoriteCurrencyCommandHandler(ICurrencyStorage currencyStorage, ICurrentUserContext userContext) : BaseCommandHandler<AddFavoriteCurrencyCommand>
{
    protected override async Task<CommandResult> ExecuteCoreAsync(AddFavoriteCurrencyCommand request, CancellationToken cancellationToken = default)
    {
        List<Error> errors = [];

        if (!Guid.TryParse(userContext.UserId, out var userId) || userId == Guid.Empty)
            errors.Add(new Error("Некорректный пользователь."));
        
        if (string.IsNullOrWhiteSpace(request.Name))
            errors.Add(new Error("Код пустой."));

        if (errors.Count > 0)
            return CommandResult.Failure(errors);

        var entity = new FavoriteCurrencyEntity
        {
            UserId = userId,
            Code = request.Name,
        };

        await currencyStorage.AddFavoriteCurrencyAsync(entity, cancellationToken);
        
        return CommandResult.Success();
    }
}