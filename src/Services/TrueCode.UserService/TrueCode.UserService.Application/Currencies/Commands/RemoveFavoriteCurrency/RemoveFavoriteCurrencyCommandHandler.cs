using TrueCode.Core.Commands;
using TrueCode.Core.Models;
using TrueCode.UserService.Domain.Dao;

namespace TrueCode.UserService.Application.Currencies.Commands.RemoveFavoriteCurrency;

public sealed class RemoveFavoriteCurrencyCommandHandler(ICurrencyStorage currencyStorage) : BaseCommandHandler<RemoveFavoriteCurrencyCommand>
{
    protected override async Task<CommandResult> ExecuteCoreAsync(RemoveFavoriteCurrencyCommand command, CancellationToken cancellationToken = default)
    {
        List<Error> errors = [];

        if (command.UserId == Guid.Empty)
            errors.Add(new Error("Пользователь не найден."));
        
        if (string.IsNullOrWhiteSpace(command.Name))
            errors.Add(new Error("Код пустой."));

        if (errors.Count > 0)
            return CommandResult.Failure(errors);
        
        await currencyStorage.RemoveFavoriteCurrencyAsync(command.UserId, command.Name, cancellationToken);
        return CommandResult.Success();
    }
}