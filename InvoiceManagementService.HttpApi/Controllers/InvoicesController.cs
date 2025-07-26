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
    public async Task<IActionResult> CreateInvoice(
        [FromForm] CreateInvoiceRequest request)
    {
        _logger.LogInformation("Received request to create invoice with InvoiceNumber: {InvoiceNumber}", request.Invoice.InvoiceNumber);

        try
        {
            var created = await _invoiceService.CreateInvoiceAsync(request.Invoice, request.File);
            return CreatedAtAction(nameof(GetInvoiceById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating invoice");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
        }
    }

    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInvoiceById(Guid id)
    {
        _logger.LogInformation("Received request to get invoice by Id: {InvoiceId}", id);

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

    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateInvoice(Guid id, [FromBody] InvoiceDto dto)
    {
        _logger.LogInformation("Received request to update invoice with Id: {InvoiceId}", id);

        // The ID from the route is the source of truth.
        dto.Id = id;

        try
        {
            var updated = await _invoiceService.UpdateInvoiceAsync(dto);
            if (updated == null)
            {
                _logger.LogWarning("Invoice with Id: {InvoiceId} not found for update", id);
                return NotFound();
            }
            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating invoice with Id: {InvoiceId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
        }
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInvoice(Guid id)
    {
        _logger.LogInformation("Received request to delete invoice with Id: {InvoiceId}", id);

        try
        {
            var deleted = await _invoiceService.DeleteInvoiceAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Invoice with Id: {InvoiceId} not found for deletion", id);
                return NotFound();
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting invoice with Id: {InvoiceId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
        }
    }
}
