using InvoiceManagementService.Application.Contracts.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace InvoiceManagementService.Infrastructure.HealthChecks;

public class AppwriteStorageHealthCheck : IHealthCheck
{
    private readonly IAppwriteStorageService _storageService;

    public AppwriteStorageHealthCheck(IAppwriteStorageService storageService)
    {
        _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            bool isConnected = await _storageService.CheckConnectionAsync(cancellationToken);
            return isConnected
                ? HealthCheckResult.Healthy("Successfully connected to Appwrite Storage.")
                : HealthCheckResult.Unhealthy("Failed to connect to Appwrite Storage. The check returned false.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("An exception occurred while checking Appwrite Storage connection.", ex);
        }
    }
}

