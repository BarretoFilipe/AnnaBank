using System.Net;

namespace AnnaBank.Abstractions
{
    public static class TransactionError
    {
        public static Error NotFound(string propertyName)
            => new Error(HttpStatusCode.NotFound, $"'{propertyName}' not found.");
        
        public static Error SenderNotHaveBalance
            => new Error(HttpStatusCode.BadRequest, $"Sender has no balance.");

        public static Error SameUser
            => new Error(HttpStatusCode.BadRequest, "The sender and receiver can't be the same.");
    }
}