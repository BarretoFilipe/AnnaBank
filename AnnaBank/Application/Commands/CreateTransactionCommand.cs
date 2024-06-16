namespace AnnaBank.Application.Commands
{
    public record CreateTransactionCommand(
        decimal Amount,
        Guid SenderId,
        string Iban
    );
}