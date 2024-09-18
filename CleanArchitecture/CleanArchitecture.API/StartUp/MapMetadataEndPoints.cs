using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Core.Entites;
using CleanArchitecture.Infrastructure.Repository;
using System.Text.Json;
using System.Xml.Linq;

namespace CleanArchitecture.API.StartUp
{
    public static class MapMetadataEndpoint
    {
        public static WebApplication MapMetadataEndpoints(this WebApplication app)
        {
            app.MapGet("metadatas", async (IUnitOfWork unitOfWork, HttpContext context) =>
            {
                var metadatas = await unitOfWork.Metadatas.GetAllAsync();

                var filteredMetadata = ApplyFilters(context, metadatas);

                return Results.Ok(filteredMetadata);
            })
            .AllowAnonymous()
            .RequireCors("CorsPolicy");

            app.MapGet("metadatas/{id}", async (long id, IUnitOfWork unitOfWork) =>
            {
                var metadata = await unitOfWork.Metadatas.GetByIdAsync(id);

                return Results.Ok(metadata);
            })
            .AllowAnonymous()
            .RequireCors("CorsPolicy");

            app.MapGet("/metadatas/filter", async (string? name, IUnitOfWork unitOfWork) =>
            {
                var metadatas = await unitOfWork.Metadatas.GetAllAsync();

                var metadataList = metadatas.ToList();

                if (!string.IsNullOrEmpty(name))
                    metadataList = metadataList.Where(s => s.name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

                return Results.Ok(metadataList.ToList());
            })
            .AllowAnonymous()
            .RequireCors("CorsPolicy");

            return app;
        }

        public static IEnumerable<Metadata> ApplyFilters(HttpContext context, IReadOnlyList<Metadata> metdatas)
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
                            metdatas = metdatas.Where(s => s.name.Contains(nameFilterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                        }
                    }

                    // Apply the id filter
                    if (filters.TryGetValue("id", out var idFilter) && idFilter.ValueKind == JsonValueKind.String)
                    {
                        if (long.TryParse(idFilter.GetString(), out var idFilterValue))
                        {
                            metdatas = metdatas.Where(s => s.id == idFilterValue).ToList();
                        }
                    }
                }
            }

            // Calculate total count for pagination header
            var totalMetadatas = metdatas.Count();

            // Apply pagination
            var metadataList = metdatas.Skip(rangeArray[0]).Take(rangeArray[1] - rangeArray[0]);

            // Set pagination headers
            context.Response.Headers.Add("Content-Range", $"metadatas {rangeArray[0]}-{rangeArray[1]}/{totalMetadatas}");
            context.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");

            return metadataList;
        }
    }
}
