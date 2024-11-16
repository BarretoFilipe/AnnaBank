using AnnaBank.Application.Commands;
using AnnaBank.Application.Validations;
using AnnaBank.Infra.Seeders;
using AnnaBank.Middlewares;
using AnnaBank.Services;
using AnnaBank.Services.Interfaces;
using AnnaBank.Shared.Extensions;
using FluentValidation;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        builder.Services.AddPersistence(builder);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddValidatorsFromAssemblyContaining<CreateTransactionCommandValidator>();

        builder.Services.AddScoped<IValidator<CreateTransactionCommand>, CreateTransactionCommandValidator>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();

        builder.Services.AddRepositories();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo()
                {
                    Title = "Anna Bank - V1",
                    Version = "v1"
                }
             );

            var filePath = Path.Combine(AppContext.BaseDirectory, "AnnaBank.xml");
            c.IncludeXmlComments(filePath);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.PopulateSeeder();

        app.MapControllers();

        app.Run();
    }
}