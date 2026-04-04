using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace ArtOnline.Infrastructure.Converters;

internal sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var requirements = new Dictionary<string, IOpenApiSecurityScheme>
        {
            [JwtBearerDefaults.AuthenticationScheme] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
                BearerFormat = "JWT"
            }
        };

        document.Components ??= new();
        document.Components.SecuritySchemes = requirements;
        document.Security = [
            new()
            {
                [new(JwtBearerDefaults.AuthenticationScheme)
                {
                    Reference = new()
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                }] = []
            }
        ];
        document.SetReferenceHostDocument();
        
        return Task.CompletedTask;
    }
}