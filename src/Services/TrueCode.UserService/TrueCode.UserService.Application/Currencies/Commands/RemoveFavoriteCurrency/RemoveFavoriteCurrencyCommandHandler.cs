using TrueCode.Core.Commands;
using TrueCode.Core.Models;
using TrueCode.Core.Users;
using TrueCode.UserService.Domain.Dao;

namespace TrueCode.UserService.Application.Currencies.Commands.RemoveFavoriteCurrency;

internal sealed class RemoveFavoriteCurrencyCommandHandler(ICurrencyStorage currencyStorage, ICurrentUserContext userContext) : BaseCommandHandler<RemoveFavoriteCurrencyCommand>
{
    protected override async Task<CommandResult> ExecuteCoreAsync(RemoveFavoriteCurrencyCommand command, CancellationToken cancellationToken = default)
    {
        List<Error> errors = [];

        if (!Guid.TryParse(userContext.UserId, out var userId) || userId == Guid.Empty)
            errors.Add(new Error("Пользователь не найден."));
        
        if (string.IsNullOrWhiteSpace(command.Name))
            errors.Add(new Error("Код пустой."));

        if (errors.Count > 0)
            return CommandResult.Failure(errors);
        
        await currencyStorage.RemoveFavoriteCurrencyAsync(userId, command.Name, cancellationToken);
        return CommandResult.Success();
    }
}