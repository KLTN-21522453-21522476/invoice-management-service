namespace InvoiceManagementService.Infrastructure.AppwriteService;

public class AppwriteConfig
{
    public const string SectionName = "Appwrite";
    public string Endpoint { get; set; } = string.Empty;
    public string ProjectId { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string BucketId { get; set; } = string.Empty;
}
