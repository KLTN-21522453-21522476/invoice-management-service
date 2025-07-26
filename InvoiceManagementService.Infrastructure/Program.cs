using Appwrite;
using InvoiceManagementService.Application.Contracts.Interfaces;
using InvoiceManagementService.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using InvoiceManagementService.Infrastructure.AppwriteService;

namespace InvoiceManagementService.Infrastructure
{
    public static class Program
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.Configure<AppwriteConfig>(
                configuration.GetSection(AppwriteConfig.SectionName));

            services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<AppwriteConfig>>().Value;
                return new Client()
                    .SetEndpoint(settings.Endpoint)
                    .SetProject(settings.ProjectId)
                    .SetKey(settings.ApiKey);
                    
            });

            services.AddScoped<IAppwriteStorageService, AppwriteStorageService>();

            return services;
        }
    }
}
