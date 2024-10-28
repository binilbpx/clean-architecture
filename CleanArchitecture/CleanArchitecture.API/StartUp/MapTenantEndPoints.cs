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

            app.MapPatch("tenants/{id}", async (long id, Tenant tenant, IUnitOfWork unitOfWork) =>
            {
                var updated_id = await unitOfWork.Tenants.UpdateAsync(tenant);

                return Results.Ok(tenant);
            })
            .AllowAnonymous();


            app.MapPost("tenants", async (Tenant tenant, IUnitOfWork unitOfWork) =>
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
            var startString = context.Request.Query["_start"].ToString();
            var endString = context.Request.Query["_end"].ToString();
            var sortString = context.Request.Query["_sort"].ToString();
            var orderString = context.Request.Query["_order"].ToString();
            var filterString = context.Request.Query["filter"].ToString();

            // Parse the _start and _end parameters
            int _start = string.IsNullOrEmpty(startString) ? 0 : int.Parse(startString);
            int _end = string.IsNullOrEmpty(endString) ? tenants.Count : int.Parse(endString);

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

                    // Apply the textToSpeechEnabled filter
                    if (filters.TryGetValue("textToSpeechEnabled", out var textToSpeechEnabled) && (textToSpeechEnabled.ValueKind == JsonValueKind.True || textToSpeechEnabled.ValueKind == JsonValueKind.False))
                    {
                        var isActivationAllowedFilterValue = textToSpeechEnabled.GetBoolean();
                        tenants = tenants.Where(s => s.textToSpeechEnabled == isActivationAllowedFilterValue).ToList();
                    }
                }
            }

            // Apply sorting if specified
            if (!string.IsNullOrEmpty(sortString) && !string.IsNullOrEmpty(orderString))
            {
                var isAscending = orderString.Equals("ASC", StringComparison.OrdinalIgnoreCase);

                tenants = sortString switch
                {
                    "name" => isAscending ? tenants.OrderBy(t => t.name).ToList() : tenants.OrderByDescending(t => t.name).ToList(),
                    "id" => isAscending ? tenants.OrderBy(t => t.id).ToList() : tenants.OrderByDescending(t => t.id).ToList(),
                    _ => tenants
                };
            }

            // Apply pagination using _start and _end
            var tenantsList = tenants.Skip(_start).Take(_end - _start);

            // Calculate total count for pagination header
            var totalTenants = tenants.Count();

            // Set pagination headers
            context.Response.Headers.Append("X-Total-Count", totalTenants.ToString());
            context.Response.Headers.Append("Access-Control-Expose-Headers", "X-Total-Count");

            return tenantsList;
        }

    }
}
