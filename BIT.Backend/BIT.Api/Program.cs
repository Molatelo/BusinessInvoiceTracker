using BIT.Api.Endpoints;
using BIT.Application;
using BIT.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Get Configurations
var globalName = builder.Configuration.GetValue<string>("GlobalName:AppName");
var allowedOrigins = builder.Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>() ?? [];
var jwtKey = builder.Configuration.GetValue<string>("JWT:Key");
var jwtIssuer = builder.Configuration.GetValue<string>("JWT:Issuer");
var jwtAudience = builder.Configuration.GetValue<string>("JWT:Audience");
var jwtExpireMinutes = builder.Configuration.GetValue<int>("JWT:ExpiryInMinutes");

// Add services to the container.
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetPreflightMaxAge(TimeSpan.FromSeconds(86400)); // 24 hours
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, _) =>
    {
        document.Info = new()
        {
            Title = globalName,
            Version = "v1",
            Description = "BIT API Documentation"
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            {
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme.",
                    In = ParameterLocation.Header
                }
            }
        };

        document.Security = [new OpenApiSecurityRequirement{
            [new OpenApiSecuritySchemeReference("Bearer") {
                Reference = new OpenApiReferenceWithDescription {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
             }] = []
        }];

        return Task.CompletedTask;
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
        .WithTitle(globalName!)
        .WithTheme(ScalarTheme.BluePlanet)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        options.Layout = ScalarLayout.Modern;
        options.HideClientButton = true;
        options.HiddenClients = true;
    });
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

UserEndpoints.MapUsersEndpoints(app);

app.MapGet("/", () => Results.Redirect("/scalar"));

await app.RunAsync();
