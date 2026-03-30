using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexora.Application.DTOs;
using Nexora.Application.Interfaces;

namespace Nexora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DocumentosController : ControllerBase
{
    private readonly IDocumentoService _documentoService;

    public DocumentosController(IDocumentoService documentoService)
    {
        _documentoService = documentoService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromQuery] string? tipo = null,
        [FromQuery] int? clienteId = null,
        [FromQuery] DateTime? fechaDesde = null,
        [FromQuery] DateTime? fechaHasta = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var documentos = await _documentoService.GetAllAsync(tipo, clienteId, fechaDesde, fechaHasta, page, pageSize);
        var total = await _documentoService.GetTotalCountAsync(tipo, clienteId, fechaDesde, fechaHasta);

        return Ok(new
        {
            data = documentos,
            total,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling(total / (double)pageSize)
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DocumentoDto>> GetById(int id)
    {
        var documento = await _documentoService.GetByIdAsync(id);
        if (documento == null) return NotFound();
        return Ok(documento);
    }

    [HttpPost]
    public async Task<ActionResult<DocumentoDto>> Create([FromBody] CrearDocumentoDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var idVendedor))
            return Unauthorized(new { mensaje = "Token inválido" });

        try
        {
            var documento = await _documentoService.CreateAsync(dto, idVendedor);
            return CreatedAtAction(nameof(GetById), new { id = documento.Id }, documento);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id}/estado")]
    public async Task<ActionResult<DocumentoDto>> UpdateEstado(int id, [FromBody] string estado)
    {
        try
        {
            var documento = await _documentoService.UpdateEstadoAsync(id, estado);
            return Ok(documento);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
