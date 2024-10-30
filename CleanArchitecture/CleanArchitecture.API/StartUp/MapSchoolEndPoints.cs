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

                var filteredSchools = ApplyFilters(context, schools);

                return Results.Ok(filteredSchools);
            })
            .AllowAnonymous();

            app.MapGet("schools/dropdown", async (IUnitOfWork unitOfWork, HttpContext context) =>
            {
                var schools = await unitOfWork.Schools.GetAllAsync();

                return Results.Ok(schools.Select(c => new { c.id, c.name }));
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

            app.MapDelete("schools/{id}", async (long id, IUnitOfWork unitOfWork) =>
            {
                await unitOfWork.Schools.DeleteAsync(id);

                return Results.Ok();
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
            var rangeString = context.Request.Query["range"].ToString();
            var filterString = context.Request.Query["filter"].ToString();

            // Parse the range parameter
            var rangeArray = rangeString.Trim('[', ']').Split(',')
                                         .Select(int.Parse)
                                         .ToArray();

            // Apply filtering based on the filter parameter
            if (!string.IsNullOrEmpty(filterString))
            {
                // Deserialize the filter JSON to extract filter values
                var filters = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(filterString);

                if (filters != null)
                {
                    // Apply the name filter
                    if (filters.TryGetValue("name", out var nameFilter) && nameFilter.ValueKind == JsonValueKind.String)
                    {
                        var nameFilterValue = nameFilter.GetString();
                        if (!string.IsNullOrEmpty(nameFilterValue))
                        {
                            schools = schools.Where(s => s.name.Contains(nameFilterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                        }
                    }

                    // Apply the id filter
                    if (filters.TryGetValue("id", out var idFilter) && idFilter.ValueKind == JsonValueKind.String)
                    {
                        if (long.TryParse(idFilter.GetString(), out var idFilterValue))
                        {
                            schools = schools.Where(s => s.id == idFilterValue).ToList();
                        }
                    }
                    // Apply the isActivationAllowed filter
                    if (filters.TryGetValue("isActivationAllowed", out var isActivationAllowedFilter) && isActivationAllowedFilter.ValueKind == JsonValueKind.True || isActivationAllowedFilter.ValueKind == JsonValueKind.False)
                    {
                        var isActivationAllowedFilterValue = isActivationAllowedFilter.GetBoolean();
                        schools = schools.Where(s => s.isActivationAllowed == isActivationAllowedFilterValue).ToList();
                    }
                }
            }

            // Calculate total count for pagination header
            var totalSchools = schools.Count();

            // Apply pagination
            var schoolsList = schools.Skip(rangeArray[0]).Take(rangeArray[1] - rangeArray[0]);

            // Set pagination headers
            context.Response.Headers.Add("Content-Range", $"schools {rangeArray[0]}-{rangeArray[1]}/{totalSchools}");
            context.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");

            return schoolsList;
        }
    }
}
