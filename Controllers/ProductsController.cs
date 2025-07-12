using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _db.Products
            .Include(p => p.Slides)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Slides = p.Slides
                    .OrderBy(s => s.OrderNumber)
                    .Select(s => new SlideDto
                    {
                        Url = s.Url,
                        OrderNumber = s.OrderNumber
                    })
                    .ToList()
            })
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id}/slides")]
    public async Task<IActionResult> GetSlidesByProductId(int id)
    {
        var product = await _db.Products
            .Include(p => p.Slides.OrderBy(s => s.OrderNumber))
            .FirstOrDefaultAsync(p => p.Id == id);
    
        if (product == null)
            return NotFound($"No product found with ID {id}");
    
        var slides = product.Slides.Select(s => new
        {
            s.Id,
            s.Url,
            s.OrderNumber
        });
    
        return Ok(slides);
    }

}
