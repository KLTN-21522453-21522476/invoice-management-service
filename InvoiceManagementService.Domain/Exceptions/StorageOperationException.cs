namespace InvoiceManagementService.Domain.Exceptions;

public class StorageOperationException : Exception
{
    public StorageOperationException(string message) : base(message) { }

    public StorageOperationException(string message, Exception innerException)
        : base(message, innerException) { }
}
