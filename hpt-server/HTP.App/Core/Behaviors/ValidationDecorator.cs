
using FluentValidation;
using FluentValidation.Results;
using HPT.SharedKernel;
using HTP.App.Core.Abstractions.Mediator;

namespace HTP.App.Core.Behaviors;

public static class ValidationDecorator
{
    public sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken ct)
        {
            var validationFailures = await ValidateAsync(command, validators);

            if (validationFailures.Length > 0)
                return Result.Failure<TResponse>(CreateValidationError(validationFailures));

            return await innerHandler.Handle(command, ct);
        }
    }

    public sealed class CommandHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken ct)
        {
            var validationFailures = await ValidateAsync(command, validators);

            if (validationFailures.Length > 0)
                return Result.Failure(CreateValidationError(validationFailures));

            return await innerHandler.Handle(command, ct);
        }
    }

    private static async Task<ValidationFailure[]> ValidateAsync<TCommand>(
        TCommand command,
        IEnumerable<IValidator<TCommand>> validators)
    {
        if (!validators.Any())
            return [];

        var context = new ValidationContext<TCommand>(command);

        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context)));

        return validationResults
            .Where(r => !r.IsValid)
            .SelectMany(r => r.Errors)
            .ToArray();
    }

    private static ValidationError CreateValidationError(ValidationFailure[] failures)
    {
        var fieldErrors = failures
            .GroupBy(f => (f.PropertyName, f.ErrorCode, f.ErrorMessage))
            .Select(g =>
            {
                var firstFailure = g.First();
                var args = ExtractArgsFromValidator(firstFailure);

                return new FieldValidationError(
                    Field: g.Key.PropertyName,
                    TranslationPhrase: g.Key.ErrorCode,
                    Message: g.Key.ErrorMessage,
                    Args: args
                );
            })
            .ToArray();

        return new ValidationError(fieldErrors);
    }


    private static Dictionary<string, object>? ExtractArgsFromValidator(ValidationFailure failure)
    {
        if (failure.FormattedMessagePlaceholderValues is not { Count: > 0 })
            return null;

        var values = failure.FormattedMessagePlaceholderValues;

        var result = new Dictionary<string, object>();

        if (values.TryGetValue("MinLength", out var minLength) && minLength is int min && min > 0)
            result["minLength"] = min;

        if (values.TryGetValue("MaxLength", out var maxLength) && maxLength is int max && max > 0)
            result["maxLength"] = max;

        if (values.TryGetValue("ExactLength", out var exactLength) && exactLength is int exact && exact > 0)
            result["exactLength"] = exact;

        if (values.TryGetValue("ComparisonValue", out var comparisonValue))
            result["comparison"] = comparisonValue;

        if (values.TryGetValue("From", out var from))
            result["from"] = from;

        if (values.TryGetValue("To", out var to))
            result["to"] = to;

        if (values.TryGetValue("ValueToCompare", out var valueToCompare))
            result["valueToCompare"] = valueToCompare;

        return result.Count > 0 ? result : null;
    }
}
