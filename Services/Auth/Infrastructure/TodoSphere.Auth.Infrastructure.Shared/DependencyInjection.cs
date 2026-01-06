using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RmqFactory = RabbitMQ.Client.ConnectionFactory;
using TodoSphere.Auth.Application.Interfaces.Services;
using TodoSphere.Auth.Application.Utils;
using TodoSphere.Auth.Domain.Settings;
using TodoSphere.Auth.Infrastructure.Shared.Messaging;
using TodoSphere.Auth.Infrastructure.Shared.Services.Auth;

namespace TodoSphere.Auth.Infrastructure.Shared;

public static class DependencyInjection
{
    public static void AddInfrastructureShared(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSetting>(configuration.GetSection("JwtSetting"));

        services.AddScoped<IJwtService, JwtService>();

        services.AddAuthenticationBearer(configuration);
    }

    public static void AddRabbitMqPublisher(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddSingleton<IConnection>(sp =>
        {
            var factory = new RmqFactory
            {
                HostName = configuration["RabbitMq:HostName"],
                UserName = configuration["RabbitMq:UserName"],
                Password = configuration["RabbitMq:Password"],
                Port = int.Parse(configuration["RabbitMq:Port"] ?? string.Empty)
            };

            return factory.CreateConnection();
        });

        builder.Services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
    }


    private static void AddAuthenticationBearer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                {
                    var jwtSettings = configuration.GetSection("JwtSetting").Get<JwtSetting>();
                    var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };

                    // Events
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = async context =>
                        {
                            var message = context.Exception switch
                            {
                                SecurityTokenExpiredException => JwtMessages.TokenExpired,
                                SecurityTokenInvalidSignatureException => JwtMessages.InvalidSignature,
                                _ => JwtMessages.AuthenticationFailed
                            };

                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";

                            var result = JsonConvert.SerializeObject(new JwtResponse(true, message));
                            await context.Response.WriteAsync(result);
                        },
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";

                            var result = JsonConvert.SerializeObject(new JwtResponse(true, JwtMessages.AccessDenied));
                            await context.Response.WriteAsync(result);
                        },

                        OnForbidden = async context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";

                            var result = JsonConvert.SerializeObject(new JwtResponse(true, JwtMessages.Forbidden));
                            await context.Response.WriteAsync(result);
                        },

                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                }
            );
    }
}