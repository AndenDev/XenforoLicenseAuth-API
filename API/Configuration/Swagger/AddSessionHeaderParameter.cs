using System.IO;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Configuration.Swagger
{
    public class AddSessionHeaderParameter : IOperationFilter
    {
        private readonly HashSet<PathString> _excludedPaths = new()
        {
            ApiRoutes.AuthRoutes.Login,
            ApiRoutes.AuthRoutes.Logout,
            ApiRoutes.AuthRoutes.ValidateSession
        };

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var relativePath = "/" + context.ApiDescription.RelativePath?.TrimEnd('/');
            if (_excludedPaths.Any(p => string.Equals(p, relativePath, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Session-Id",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Description = "The session ID returned from the Login API."
                }
            });
        }
    }
}
