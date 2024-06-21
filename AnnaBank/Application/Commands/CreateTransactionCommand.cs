namespace AnnaBank.Application.Commands
{
    public sealed record CreateTransactionCommand(
        decimal Amount,
        Guid SenderId,
        string Iban
    );
}