using CSharpApp.Application.Authentication;
using CSharpApp.Application.Categories;
using CSharpApp.Application.Products;
using Polly.Extensions.Http;
using Polly;
using CSharpApp.Application;

namespace CSharpApp.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            // Add services to the container.
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();
            services.AddDefaultConfiguration();
            services.AddHttpConfiguration();
            services.AddProblemDetails();
            services.AddApiVersioning();

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(120);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddHttpContextAccessor();

            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IProductsMediatorService, ProductsMediatorService>();
            services.AddTransient<ICategoriesMediatorService, CategoriesMediatorService>();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(MediatorEntryPoint).Assembly);
            });

            return services;
        }
        public static IServiceCollection AddProjectHttpClients(this IServiceCollection services, IConfiguration config)
        {
            string productApiBaseAddress = config.GetValue<string>("RestApiSettings:BaseUrl") ?? string.Empty;

            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(12));

            services.AddHttpClient<IProductsService, ProductsService>(client =>
            {
                client.BaseAddress = new Uri(productApiBaseAddress);
            })
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);


            services.AddHttpClient<ICategoriesService, CategoriesService>(client =>
            {
                client.BaseAddress = new Uri(productApiBaseAddress);
            })
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);


            services.AddHttpClient<IAuthService, AuthService>(client =>
            {
                client.BaseAddress = new Uri(productApiBaseAddress);
            })
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);

            return services;
        }
    }
}
