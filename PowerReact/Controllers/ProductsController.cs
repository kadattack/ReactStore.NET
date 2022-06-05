using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerReact.Data;
using PowerReact.Entities;

namespace PowerReact.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly DataContext _context;
    public ProductsController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }

    [HttpGet("{id}")] //api/products/3
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return BadRequest("No product of this id");

        return product;
    }
}