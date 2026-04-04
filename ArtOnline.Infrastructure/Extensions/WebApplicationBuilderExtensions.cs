using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.Json;
using ArtOnline.Infrastructure.Configurations;
using ArtOnline.Infrastructure.Converters;
using ArtOnline.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;

namespace ArtOnline.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    extension(WebApplicationBuilder builder)
    {
        /// <summary>
        /// This extension method adds the CORS configuration to the application builder.
        /// </summary>
        public WebApplicationBuilder AddCorsConfiguration()
        {
            var corsConfiguration = builder.Configuration.GetRequiredSection(nameof(CorsConfiguration)).Get<CorsConfiguration>();

            if (corsConfiguration == null)
            {
                throw new ApplicationException("The CORS configuration needs to be set!");
            }
        
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policyBuilder =>
                    {
                        policyBuilder.WithOrigins(corsConfiguration.Origins) // This adds the valid origins that the browser client can have.
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            return builder;
        }

        /// <summary>
        /// This extension method adds the advanced logging configuration to the application builder.
        /// </summary>
        public WebApplicationBuilder UseLogger()
        {
            builder.Host.UseSerilog((_, logger) =>
            {
                logger
                    .MinimumLevel.Is(LogEventLevel.Information)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithProcessId()
                    .Enrich.WithProcessName()
                    .Enrich.WithThreadId()
                    .WriteTo.Console();
            });

            return builder;
        }

        /// <summary>
        /// This extension method adds the controllers and JSON serialization configuration to the application builder.
        /// </summary>
        public WebApplicationBuilder AddApi()
        {
            builder.Services.AddControllers(options => { options.Conventions.Add(new MultipartFormConvention()); });
            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.SerializerOptions.PropertyNameCaseInsensitive = true;
                options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
            });
            builder.Services.AddEndpointsApiExplorer()
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Adds a conversion by name of the enums, otherwise numbers representing the enum values are used.
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // This converts the public property names of the objects serialized to Camel case.
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // When deserializing request the properties of the JSON are mapped ignoring the casing.
                    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict; // Only numbers are mapped to numeric field, not strings.
                });

            return builder;
        }
        
        /// <summary>
        /// This extension method adds just the authorization configuration to the application builder.
        /// </summary>
        private WebApplicationBuilder ConfigureAuthentication()
        {
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // This is to use the JWT token with the "Bearer" scheme
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    var jwtConfiguration = builder.Configuration.GetSection(nameof(JwtConfiguration)).Get<JwtConfiguration>(); // Here we use the JWT configuration from the application.json.

                    if (jwtConfiguration == null)
                    {
                        throw new ApplicationException("The JWT configuration needs to be set!");
                    }
                
                    var key = Encoding.ASCII.GetBytes(jwtConfiguration.Key); // Use configured key to verify the JWT signature.
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true, // Validate the issuer claim in the JWT. 
                        ValidateAudience = true, // Validate the audience claim in the JWT.
                        ValidAudience = jwtConfiguration.Audience, // Sets the intended audience.
                        ValidIssuer = jwtConfiguration.Issuer, // Sets the issuing authority.
                        ClockSkew = TimeSpan.Zero // No clock skew is added, when the token expires it will immediately become unusable.
                    };
                    options.RequireHttpsMetadata = false;
                    options.IncludeErrorDetails = true;
                }).Services
                .AddAuthorization(options =>
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder().AddDefaultPolicy().Build(); // Adds the default policy for the JWT claims.
                });

            return builder;
        }

        /// <summary>
        /// This extension method adds the authorization with the Swagger configuration to the application builder.
        /// </summary>
        public WebApplicationBuilder AddAuthorizationWithSwagger(string application)
        {
            builder.Services
                .AddSwaggerGen()
                .AddOpenApi(options =>
                {
                    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                    options.AddSchemaTransformer((schema, context, cancellationToken) =>
                    {
                        var jsonTypeInfo = context.JsonTypeInfo;
                        if (schema.Properties is null)
                        {
                            return Task.CompletedTask;
                        }

                        foreach (var prop in jsonTypeInfo.Properties)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }

                            if ((prop.Get is not null && prop.IsGetNullable) ||
                                (prop.Set is not null && prop.IsSetNullable) ||
                                (prop.PropertyType.IsGenericType &&
                                 prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                continue;
                            }

                            schema.Required ??= new HashSet<string>();
                            schema.Required.Add(prop.Name);
                        }

                        return Task.CompletedTask;
                    });
                });

            return builder.ConfigureAuthentication();
        }
    }
    
    /// <summary>
    /// This extension method adds the default authorization policy to the AuthorizationPolicyBuilder.
    /// It requires that the JWT needs to have the given claims in the configuration.
    /// </summary>
    private static AuthorizationPolicyBuilder AddDefaultPolicy(this AuthorizationPolicyBuilder policy) =>
        policy.RequireClaim(ClaimTypes.NameIdentifier)
            .RequireClaim(ClaimTypes.Name)
            .RequireClaim(ClaimTypes.Email);
}