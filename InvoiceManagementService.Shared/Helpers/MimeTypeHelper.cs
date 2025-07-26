namespace InvoiceManagementService.Shared.Helpers
{
    public static class MimeTypeHelper
    {
        private static readonly Dictionary<string, string> MimeTypes = new()
    {
        { ".pdf", "application/pdf" },
        { ".doc", "application/msword" },
        { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        { ".xls", "application/vnd.ms-excel" },
        { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".png", "image/png" },
        { ".gif", "image/gif" },
        { ".txt", "text/plain" },
        { ".json", "application/json" },
        { ".zip", "application/zip" }
    };

        public static string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return MimeTypes.TryGetValue(extension, out var mimeType)
                ? mimeType
                : "application/octet-stream"; // Default cho unknown files
        }
    }

}
