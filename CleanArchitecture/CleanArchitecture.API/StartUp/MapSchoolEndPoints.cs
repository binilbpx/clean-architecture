using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Core.Entites;
using CleanArchitecture.Infrastructure.Repository;
using System.Text.Json;
using System.Xml.Linq;

namespace CleanArchitecture.API.StartUp
{
    public static class MapSchoolEndpoint
    {
        public static WebApplication MapSchoolEndpoints(this WebApplication app)
        {
            app.MapGet("schools", async (IUnitOfWork unitOfWork, HttpContext context) =>
            {
                var schools = await unitOfWork.Schools.GetAllAsync();

                // Get the name_like query parameter from the URL
                var nameLikeQuery = context.Request.Query["name_like"].ToString();

                // Filter schools based on the name_like query if provided
                if (!string.IsNullOrWhiteSpace(nameLikeQuery))
                {
                    schools = schools
                        .Where(s => s.name.Contains(nameLikeQuery, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var filteredSchools = ApplyFilters(context, schools);
                return Results.Ok(filteredSchools);
            })
 .AllowAnonymous();




            app.MapGet("schools/{id}", async (long id, IUnitOfWork unitOfWork) =>
            {
                var school = await unitOfWork.Schools.GetByIdAsync(id);

                return Results.Ok(school);
            })
            .AllowAnonymous();

            app.MapPost("schools", async (School school, IUnitOfWork unitOfWork) =>
            {
                var schools = await unitOfWork.Schools.GetAllAsync();
                school.id = schools.Max(c => c.id) + 1;

                var updated_id = await unitOfWork.Schools.AddAsync(school);

                return Results.Ok(school);
            })
            .AllowAnonymous();

            app.MapPut("schools/{id}", async (long id, School school, IUnitOfWork unitOfWork) =>
            {
                var updated_id = await unitOfWork.Schools.UpdateAsync(school);

                return Results.Ok(school);
            })
            .AllowAnonymous();
            app.MapPatch("schools/{id}", async (long id, School school, IUnitOfWork unitOfWork) =>
            {
                var updated_id = await unitOfWork.Schools.UpdateAsync(school);
                return Results.Ok(school);
            })
            .AllowAnonymous();

            app.MapGet("/schools/filter", async (string? name, IUnitOfWork unitOfWork) =>
            {
                var schoolsList = await unitOfWork.Schools.GetAllAsync();

                var schools = schoolsList.ToList();

                if (!string.IsNullOrEmpty(name))
                    schools = schools.Where(s => s.name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

                return Results.Ok(schools.ToList());
            })
            .AllowAnonymous();

            return app;
        }

        public static IEnumerable<School> ApplyFilters(HttpContext context, IReadOnlyList<School> schools)
        {
            var startString = context.Request.Query["_start"].ToString();
            var endString = context.Request.Query["_end"].ToString();
            var isActivationAllowedString = context.Request.Query["isActivationAllowed"].ToString();

            // Parse the _start and _end parameters
            int _start = string.IsNullOrEmpty(startString) ? 0 : int.Parse(startString);
            int _end = string.IsNullOrEmpty(endString) ? schools.Count : int.Parse(endString);

            // Filter by isActivationAllowed if the parameter is provided
            var filteredSchools = schools;
            if (!string.IsNullOrEmpty(isActivationAllowedString) && bool.TryParse(isActivationAllowedString, out bool isActivationAllowed))
            {
                filteredSchools = filteredSchools.Where(school => school.isActivationAllowed == isActivationAllowed).ToList();
            }

            // Apply pagination using _start and _end on the filtered list
            var paginatedSchools = filteredSchools.Skip(_start).Take(_end - _start);

            // Calculate total count for pagination header
            var totalSchools = filteredSchools.Count();

            // Set pagination headers
            context.Response.Headers.Append("X-Total-Count", totalSchools.ToString());
            context.Response.Headers.Append("Access-Control-Expose-Headers", "X-Total-Count");

            return paginatedSchools;
        }

    }
}
