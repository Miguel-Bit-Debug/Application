using Application.Domain.Models;
using Application.Domain.Repositories;
using Application.Domain.Services;
using AspNetCore.Identity.Mongo;
using Infra.Data.Data;
using Infra.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Application.API.DI;

public static class DependencyInjection
{
    public static void AddDependencyInjection(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IDbContext, DbContext>();
        builder.Services.AddScoped<IProductRepository<Product>, ProductRepository>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<ITokenService, TokenService>();

        var key = Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"]);

        builder.Services.AddIdentity<Account, ApplicationRole>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
        }).AddMongoDbStores<Account, ApplicationRole, Guid>(
            builder.Configuration["MongoConnection"],
            builder.Configuration["DBName"])
        .AddDefaultTokenProviders();

        builder.Services.AddAuthorization(opt =>
        {
            opt.FallbackPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            opt.RequireHttpsMetadata = false;
            opt.SaveToken = true;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = builder.Configuration["Audience"],
                ValidIssuer = builder.Configuration["Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
    }
}
