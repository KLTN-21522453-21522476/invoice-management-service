using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementService.Application.Contracts.DTO;

public class FileUploadResult
{
    public string? FileId { get; set; }

    public string? FileUrl { get; set; }
}
