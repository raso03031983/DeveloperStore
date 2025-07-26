using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Application.Filters;
using DeveloperStore.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeveloperStore.Sales.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    /// <summary>
    /// Cria uma nova venda
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SaleDto saleDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var saleId = await _saleService.CreateAsync(saleDto, UserLoged());
        return CreatedAtAction(nameof(GetById), new { id = saleId }, new { id = saleId });
    }

    /// <summary>
    /// Retorna uma venda por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var sale = await _saleService.GetByIdAsync(id);
        if (sale == null)
            return NotFound();

        return Ok(sale);
    }

    /// <summary>
    /// Retorna todas as vendas
    /// </summary>
    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] DateTime? startDate,
    [FromQuery] DateTime? endDate,
    [FromQuery] string? customerName,
    [FromQuery] string? branchName,
    [FromQuery] bool? cancelled,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? orderBy = null,
    [FromQuery] string sortDirection = "asc")
    {
        var filter = new SaleFilter
        {
            StartDate = startDate,
            EndDate = endDate,
            CustomerName = customerName,
            BranchName = branchName,
            Cancelled = cancelled,
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrderBy = orderBy,
            SortDirection = sortDirection
        };

        var sales = await _saleService.GetAllAsync(filter);
        return Ok(sales);
    }


    /// <summary>
    /// Atualiza uma venda
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SaleDto saleDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _saleService.UpdateAsync(id, saleDto, UserLoged());
        return NoContent();
    }

    /// <summary>
    /// Cancela uma venda
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _saleService.CancelAsync(id, UserLoged());
        return NoContent();
    }

    private UserDto UserLoged()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        var userCreate = new UserDto
        {
            UserId = userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty,
            Username = User.Identity?.Name
        };

        return userCreate;
    }
}
