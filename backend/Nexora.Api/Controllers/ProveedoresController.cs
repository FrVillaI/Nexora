using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexora.Application.DTOs;
using Nexora.Application.Interfaces;

namespace Nexora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProveedoresController : ControllerBase
{
    private readonly IProveedorService _proveedorService;

    public ProveedoresController(IProveedorService proveedorService)
    {
        _proveedorService = proveedorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProveedorDto>>> GetAll()
    {
        return Ok(await _proveedorService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProveedorDto>> GetById(int id)
    {
        var proveedor = await _proveedorService.GetByIdAsync(id);
        if (proveedor == null) return NotFound();
        return Ok(proveedor);
    }

    [HttpPost]
    public async Task<ActionResult<ProveedorDto>> Create([FromBody] CrearProveedorDto dto)
    {
        var proveedor = await _proveedorService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = proveedor.Id }, proveedor);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProveedorDto>> Update(int id, [FromBody] CrearProveedorDto dto)
    {
        var proveedor = await _proveedorService.UpdateAsync(id, dto);
        return Ok(proveedor);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _proveedorService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
