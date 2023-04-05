using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Core.Entites;
using FluentValidation;
using FluentValidation.Results;

namespace CleanArchitecture.API.StartUp
{
    public static class MapEndpoints
    {
        public static WebApplication MapContactEndpoints(this WebApplication app)
        {
            app.MapPost("contact", async (IValidator<Contact> validator, Contact contact, IContactRepository _contactRepository) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(contact);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                await _contactRepository.AddAsync(contact);

                return Results.Ok(contact);
            })
            .AllowAnonymous();

            return app;
        }
    }
}
