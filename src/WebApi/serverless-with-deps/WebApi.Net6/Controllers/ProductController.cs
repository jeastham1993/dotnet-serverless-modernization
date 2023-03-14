using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Net6.Products;

namespace WebApi.Net6.Controllers;

[Route("/api/products")]
public class ProductController : ControllerBase
{
    private readonly DataContext _context;

    public ProductController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var products = await this._context.Products.ToListAsync();

        return this.Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDTO product)
    {
        var newProduct = new Product()
        {
            Name = product.Name,
            Price = product.Price
        };
        
        await this._context.Products.AddAsync(newProduct);
        await this._context.SaveChangesAsync();

        return this.Ok(newProduct);
    }
}