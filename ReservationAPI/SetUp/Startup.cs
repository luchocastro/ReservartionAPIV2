using Microsoft.AspNetCore.Http;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using global::ReservationAPI.Application.IntegrationEvents;
using global::ReservationAPI.Application.IntegrationEvents.EventHandling;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using global::Microsoft.AspNetCore.Connections;
using global::Microsoft.AspNetCore.Diagnostics.HealthChecks;
using global::Microsoft.AspNetCore.Mvc;
using global::Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ReservationAPI.Application.Controllers;
using ReservationAPI.Infrastructure.AutoFacModule;
using ReservationAPI.Infrastructure.Context;
using System.Data.Common;

 namespace ReservationAPI.SetUp;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public virtual IServiceProvider ConfigureServices(IServiceCollection services)
    {


        services
            .AddApplicationInsights(Configuration)
            .AddCustomMvc()
            .AddHealthChecks(Configuration)
            .AddCustomDbContext(Configuration)
            .AddCustomSwagger(Configuration)
            .AddCustomIntegrations(Configuration)
            .AddControllers();
                ;
        var container = new ContainerBuilder();
        container.Populate(services);
        DependencyConteiner.AddDependencies(services, Configuration);

        //configure autofac

        
        container.RegisterModule(new MediatorModule());
        container.RegisterModule(new ApplicationModule(Configuration["ConnectionString"]));
        

        return new AutofacServiceProvider(container.Build());
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        //loggerFactory.AddAzureWebAppDiagnostics();
        //loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Trace);




        var pathBase = Configuration["PATH_BASE"];
        if (!string.IsNullOrEmpty(pathBase))
        {
            loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
            app.UsePathBase(pathBase);
        }

        app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{(!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty)}/swagger/v1/swagger.json", "Ordering.API V1");
                c.OAuthClientId("orderingswaggerui");
                c.OAuthAppName("Reservation System UI");
            });

        app.UseRouting();
        ConfigureAuth(app);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapControllers();
            endpoints.MapGet("/_proto/", async ctx =>
            {
                ctx.Response.ContentType = "text/plain";
                using var fs = new FileStream(Path.Combine(env.ContentRootPath, "Proto", "basket.proto"), FileMode.Open, FileAccess.Read);
                using var sr = new StreamReader(fs);
                while (!sr.EndOfStream)
                {
                    var line = await sr.ReadLineAsync();
                    if (line != "/* >>" || line != "<< */")
                    {
                        await ctx.Response.WriteAsync(line);
                    }
                }
            });
            endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });
        });

        ConfigureEventBus(app);
    }


    private void ConfigureEventBus(IApplicationBuilder app)
    {
        //var eventBus = app.ApplicationServices.GetRequiredService<BuildingBlocks.EventBus.Abstractions.
        //>();


    }

    protected virtual void ConfigureAuth(IApplicationBuilder app)
    {
    }
}

static class CustomExtensionsMethods
{
    public static IServiceCollection AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
    {
       
        return services;
    }

    public static IServiceCollection AddCustomMvc(this IServiceCollection services)
    {

        // Added for functional tests
        //services.AddApplicationPart(typeof(ReservationController).Assembly)
        //    .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

        

        return services;
    }

    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();

        hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

        //hcBuilder
        //    .AddSqlite(
        //        configuration["ConnectionString"]);

        //if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
        //{
        //    hcBuilder
        //        .AddAzureServiceBusTopic(
        //            configuration["EventBusConnection"],
        //            topicName: "eshop_event_bus",
        //            name: "ordering-servicebus-check",
        //            tags: new string[] { "servicebus" });
        //}
        //else
        //{
        //    hcBuilder
        //        .AddRabbitMQ(
        //            $"amqp://{configuration["EventBusConnection"]}",
        //            name: "ordering-rabbitmqbus-check",
        //            tags: new string[] { "rabbitmqbus" });
        //}

        return services;
    }

    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WriteReservationContext>(options =>
        {
            options.UseSqlite(configuration["ConnectionString"] ?? "Data Source=reservation.db");
        },
                    ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                );

        //desde acá se corre la migracion
        //services.AddDbContext<IntegrationEventLogContext>(options =>
        //{
        //    options.UseSqlServer(configuration["ConnectionString"],
        //                            sqlServerOptionsAction: sqlOptions =>
        //                            {
        //                                sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
        //                                //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
        //                                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        //                            });
        //});

        return services;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "eShopOnContainers - Ordering HTTP API",
                Version = "v1",
                Description = "The Ordering Service HTTP API"
            });
            //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            //{
            //    Type = SecuritySchemeType.OAuth2,
            //    Flows = new OpenApiOAuthFlows()
            //    {
            //        Implicit = new OpenApiOAuthFlow()
            //        {
            //            AuthorizationUrl = new Uri($"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize"),
            //            TokenUrl = new Uri($"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/token"),
            //            Scopes = new Dictionary<string, string>()
            //            {
            //                { "orders", "Ordering API" }
            //            }
            //        }
            //    }
            //});

            //options.OperationFilter<AuthorizeCheckOperationFilter>();
        });

        return services;
    }

    public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //services.AddTransient<IIdentityService, IdentityService>();
        //services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
        //    sp => (DbConnection c) => new IntegrationEventLogService(c));

        //services.AddTransient<IOrderingIntegrationEventService, OrderingIntegrationEventService>();

        //if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
        //{
        //    services.AddSingleton<IServiceBusPersisterConnection>(sp =>
        //    {
        //        var serviceBusConnectionString = configuration["EventBusConnection"];

        //        var subscriptionClientName = configuration["SubscriptionClientName"];

        //        return new DefaultServiceBusPersisterConnection(serviceBusConnectionString);
        //    });
        //}
        //else
        //{
        //    services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
        //    {
        //        var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();


        //        var factory = new ConnectionFactory()
        //        {
        //            HostName = configuration["EventBusConnection"],
        //            DispatchConsumersAsync = true
        //        };

        //        if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
        //        {
        //            factory.UserName = configuration["EventBusUserName"];
        //        }

        //        if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
        //        {
        //            factory.Password = configuration["EventBusPassword"];
        //        }

        //        var retryCount = 5;
        //        if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
        //        {
        //            retryCount = int.Parse(configuration["EventBusRetryCount"]);
        //        }

        //        return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
        //    });
        //}

        return services;
    }

    //public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
    //{
    //    services.AddOptions();
    //    services.Configure<ReservationSettings>(configuration);
    //    services.Configure<ApiBehaviorOptions>(options =>
    //    {
    //        options.InvalidModelStateResponseFactory = context =>
    //        {
    //            var problemDetails = new ValidationProblemDetails(context.ModelState)
    //            {
    //                Instance = context.HttpContext.Request.Path,
    //                Status = StatusCodes.Status400BadRequest,
    //                Detail = "Please refer to the errors property for additional details."
    //            };

    //            return new BadRequestObjectResult(problemDetails)
    //            {
    //                ContentTypes = { "application/problem+json", "application/problem+xml" }
    //            };
    //        };
    //    });

    //    return services;
    //}

    //public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    //{
    //    if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
    //    {
    //        services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
    //        {
    //            var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
    //            var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
    //            var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
    //            var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
    //            string subscriptionName = configuration["SubscriptionClientName"];

    //            return new EventBusServiceBus(serviceBusPersisterConnection, logger,
    //                eventBusSubcriptionsManager, iLifetimeScope, subscriptionName);
    //        });
    //    }
    //    else
    //    {
    //        services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
    //        {
    //            var subscriptionClientName = configuration["SubscriptionClientName"];
    //            var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
    //            var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
    //            var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
    //            var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

    //            var retryCount = 5;
    //            if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
    //            {
    //                retryCount = int.Parse(configuration["EventBusRetryCount"]);
    //            }

    //            return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
    //        });
    //    }

    //    services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

    //    return services;
    //}

    //public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    //{
    //    // prevent from mapping "sub" claim to nameidentifier.
    //    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

    //    var identityUrl = configuration.GetValue<string>("IdentityUrl");

    //    services.AddAuthentication("Bearer").AddJwtBearer(options =>
    //    {
    //        options.Authority = identityUrl;
    //        options.RequireHttpsMetadata = false;
    //        options.Audience = "orders";
    //        options.TokenValidationParameters.ValidateAudience = false;
    //    });

    //    return services;
    //}
    //public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, IConfiguration configuration)
    //{
    //    services.AddAuthorization(options =>
    //    {
    //        options.AddPolicy("ApiScope", policy =>
    //        {
    //            policy.RequireAuthenticatedUser();
    //            policy.RequireClaim("scope", "orders");
    //        });
    //    });
    //    return services;
    //}
}