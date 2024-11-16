using AnnaBank.Abstractions;
using AnnaBank.Application.Commands;
using AnnaBank.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AnnaBank.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IValidator<CreateTransactionCommand> _validator;
        private readonly ITransactionService _transactionService;

        public TransactionController(IValidator<CreateTransactionCommand> validator,
            ITransactionService transactionService)
        {
            _validator = validator;
            _transactionService = transactionService;
        }

        /// <summary>
        /// Transfer money between bank clients
        /// </summary>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CreateTransactionCommand createTransactionCommand)
        {
            var validationResult = await _validator.ValidateAsync(createTransactionCommand);
            if (!validationResult.IsValid)
            {
                Error errorDTO = new(HttpStatusCode.BadRequest, validationResult.ToString());
                return BadRequest(errorDTO);
            }

            Result result = await _transactionService.GenerateTransaction(createTransactionCommand);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }
    }
}