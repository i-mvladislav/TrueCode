using TrueCode.Core.Enums;

namespace TrueCode.Core.Models;

public sealed record Error(string Message, ErrorType ErrorType = ErrorType.Validation);