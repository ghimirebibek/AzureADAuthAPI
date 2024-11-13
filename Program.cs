using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

Env.Load();

var builder = WebApplication.CreateBuilder(args);
// Load environment variables for Azure AD
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(
        jwtBearerOptions => { },  // Configure JwtBearerOptions if needed
        microsoftIdentityOptions =>
        {
            // Set Microsoft Identity options using environment variables
            microsoftIdentityOptions.Instance = Environment.GetEnvironmentVariable("AZURE_AD_INSTANCE")!;
            microsoftIdentityOptions.Domain = Environment.GetEnvironmentVariable("AZURE_AD_DOMAIN")!;
            microsoftIdentityOptions.TenantId = Environment.GetEnvironmentVariable("AZURE_AD_TENANT_ID")!;
            microsoftIdentityOptions.ClientId = Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_ID")!;
        });

// Register controllers
builder.Services.AddControllers();

// Configure Swagger to use Bearer authentication
builder.Services.AddSwaggerGen(options =>
{
    // Add the JWT Bearer token authentication to Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter JWT Bearer token"
    });

    // Make the Bearer authentication required for all operations
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Enable Swagger and Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.OAuthClientId(Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_ID")); // Optional, for OAuth2
        c.OAuthClientSecret(Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_SECRET")); // Optional
    });
}

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();  // Enable authorization middleware

app.MapControllers(); // Map API controllers

app.Run();
