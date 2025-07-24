using InvoiceManagementService.Application.Contracts.DTO;
using InvoiceManagementService.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceManagementService.HttpApi.Controllers;

[ApiController]
[Route("api/invoice")]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;
    private readonly ILogger<InvoicesController> _logger;

    public InvoicesController(IInvoiceService invoiceService, ILogger<InvoicesController> logger)
    {
        _invoiceService = invoiceService;
        _logger = logger;
    }

    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] InvoiceDto dto)
    {
        _logger.LogInformation("Received request to create invoice with InvoiceNumber: {InvoiceNumber}", dto.InvoiceNumber);
        if (dto == null)
        {
            _logger.LogWarning("Received null InvoiceDto");
            return BadRequest("Invoice data cannot be null");
        }
        if (string.IsNullOrWhiteSpace(dto.InvoiceNumber))
        {
            _logger.LogWarning("InvoiceNumber is required");
            return BadRequest("InvoiceNumber is required");
        }

        try
        {
            var created = await _invoiceService.CreateInvoiceAsync(dto);
            return CreatedAtAction(nameof(GetInvoiceById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating invoice");
            return StatusCode(StatusCodes.Status500InternalServerError, "Invalid invoice data");
        }

    }

    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInvoiceById(Guid id)
    {
        _logger.LogInformation("Received request to get invoice by Id: {InvoiceId}", id);
        if (id == Guid.Empty)
        {
            _logger.LogWarning("Invalid Id provided: {InvoiceId}", id);
            return BadRequest("Invalid Id");
        }
        try
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                _logger.LogWarning("Invoice with Id: {InvoiceId} not found", id);
                return NotFound();
            }
            _logger.LogInformation("Invoice with Id: {InvoiceId} retrieved successfully", id);
            return Ok(invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving invoice with Id: {InvoiceId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
        }
    }
}
