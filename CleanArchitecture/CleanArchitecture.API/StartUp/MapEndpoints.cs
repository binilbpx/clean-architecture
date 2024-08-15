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
            app.MapPost("contact", async (IValidator<Contact> validator, Contact contact, IUnitOfWork unitOfWork) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(contact);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                await unitOfWork.Contacts.AddAsync(contact);

                return Results.Ok(contact);
            })
            .AllowAnonymous();

            app.MapGet("schools", async (IUnitOfWork unitOfWork) => 
            {
                var schools = await unitOfWork.Schools.GetAllAsync();

                return Results.Ok(schools);
            })
            .AllowAnonymous();

            app.MapGet("tenants", async (IUnitOfWork unitOfWork) =>
            {
                var schools = await unitOfWork.Schools.GetAllAsync();

                var tenants = schools.Select(c => c.tenant).Distinct();

                return Results.Ok(tenants);
            })
            .AllowAnonymous();

            app.MapGet("schools/{id}", async (long id, IUnitOfWork unitOfWork) =>
            {
                var school = await unitOfWork.Schools.GetByIdAsync(id);

                return Results.Ok(school);
            })
            .AllowAnonymous();

            app.MapGet("/schools/search", async (string? name, bool? isActive, int? tenantId, IUnitOfWork unitOfWork) =>
            {
                var schoolsList = await unitOfWork.Schools.GetAllAsync();

                var schools = schoolsList.ToList();

                if (!string.IsNullOrEmpty(name))
                    schools = schools.Where(s => s.name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

                if (isActive.HasValue)
                    schools = schools.Where(s => s.isActive == isActive.Value).ToList();

                if (tenantId.HasValue)
                    schools = schools.Where(s => s.tenantId == tenantId.Value).ToList();

                return Results.Ok(schools.ToList());
            })
            .AllowAnonymous();

            return app;
        }
    }
}
