using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Core.Entites;
using CleanArchitecture.Infrastructure.Repository;
using System.Text.Json;
using System.Xml.Linq;

namespace CleanArchitecture.API.StartUp
{
    public static class MapTenantEndpoint
    {
        public static WebApplication MapTenantEndpoints(this WebApplication app)
        {
            app.MapGet("tenants", async (IUnitOfWork unitOfWork, HttpContext context) =>
            {
                var tenants = await unitOfWork.Tenants.GetAllAsync();

                var filteredTenants = ApplyFilters(context, tenants);

                return Results.Ok(filteredTenants);
            })
            .AllowAnonymous();

            app.MapGet("tenants/{id}", async (long id, IUnitOfWork unitOfWork) =>
            {
                var tenant = await unitOfWork.Tenants.GetByIdAsync(id);

                return Results.Ok(tenant);
            })
            .AllowAnonymous();

            app.MapPut("tenant/{id}", async (long id, Tenant tenant, IUnitOfWork unitOfWork) =>
            {
                var updated_id = await unitOfWork.Tenants.UpdateAsync(tenant);

                return Results.Ok(tenant);
            })
            .AllowAnonymous();

            app.MapPost("tenant", async (Tenant tenant, IUnitOfWork unitOfWork) =>
            {
                var tenants = await unitOfWork.Tenants.GetAllAsync();
                tenant.id = tenants.Max(c => c.id) + 1;

                var updated_id = await unitOfWork.Tenants.AddAsync(tenant);

                return Results.Ok(tenant);
            })
            .AllowAnonymous();

            app.MapGet("/tenants/filter", async (string? name, IUnitOfWork unitOfWork) =>
            {
                var tenantsList = await unitOfWork.Tenants.GetAllAsync();

                var tenants = tenantsList.ToList();

                if (!string.IsNullOrEmpty(name))
                    tenants = tenants.Where(s => s.name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

                return Results.Ok(tenants.ToList());
            })
            .AllowAnonymous();

            return app;
        }

        public static IEnumerable<Tenant> ApplyFilters(HttpContext context, IReadOnlyList<Tenant> tenants)
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
                            tenants = tenants.Where(s => s.name.Contains(nameFilterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                        }
                    }

                    // Apply the id filter
                    if (filters.TryGetValue("id", out var idFilter) && idFilter.ValueKind == JsonValueKind.String)
                    {
                        if (long.TryParse(idFilter.GetString(), out var idFilterValue))
                        {
                            tenants = tenants.Where(s => s.id == idFilterValue).ToList();
                        }
                    }
                    // Apply the isActivationAllowed filter
                    if (filters.TryGetValue("textToSpeechEnabled", out var textToSpeechEnabled) && textToSpeechEnabled.ValueKind == JsonValueKind.True || textToSpeechEnabled.ValueKind == JsonValueKind.False)
                    {
                        var isActivationAllowedFilterValue = textToSpeechEnabled.GetBoolean();
                        tenants = tenants.Where(s => s.textToSpeechEnabled == isActivationAllowedFilterValue).ToList();
                    }
                }
            }

            // Calculate total count for pagination header
            var totalTenants = tenants.Count();

            // Apply pagination
            var tenantsList = tenants.Skip(rangeArray[0]).Take(rangeArray[1] - rangeArray[0]);

            // Set pagination headers
            context.Response.Headers.Append("Content-Range", $"tenants {rangeArray[0]}-{rangeArray[1]}/{totalTenants}");
            context.Response.Headers.Append("Access-Control-Expose-Headers", "Content-Range");

            return tenantsList;
        }
    }
}
