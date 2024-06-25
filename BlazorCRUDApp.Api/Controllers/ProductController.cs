using BlazorCRUDApp.Api.Entities;
using BlazorCRUDApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorCRUDApp.Api.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly BlazorCRUDDbContext _context;

        public ProductController(BlazorCRUDDbContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            if (_context.Product == null)
            {
                return NotFound();
            }

            if (_context.Category == null)
            {
                return NotFound();
            }

            var productEntities = await _context.Product.Include(x => x.Category).ToListAsync();

            var products = productEntities.Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Amount = x.Amonut,
                Price = x.Price,
                CategoryGuid = x.CategoryGuid,
                CategoryName = x.Category?.Name ?? string.Empty,
            }).OrderBy(x => x.Id);

            return Ok(products);
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            if (_context.Product == null)
            {
                return NotFound();
            }
            var productEntity = await _context.Product.FindAsync(id);

            if (productEntity == null)
            {
                return NotFound();
            }

            var product = new ProductDto
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Amount = productEntity.Amonut,
                Price = productEntity.Price,
                CategoryGuid = productEntity.CategoryGuid,
            };

            return Ok(product);
        }

        [HttpGet("name")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryDto queryDto)
        {
            if (_context.Product == null)
            {
                return NotFound();
            }

            var productEntities = new List<ProductEntity>();
            if (!string.IsNullOrWhiteSpace(queryDto.Name) && queryDto.CategoryGuid.HasValue && queryDto.CategoryGuid.Value != Guid.Empty)
            {
                productEntities = await _context.Product.Include(x => x.Category).Where(x => x.Name.Contains(queryDto.Name) && x.CategoryGuid == queryDto.CategoryGuid.Value).ToListAsync();
            }
            else if (!string.IsNullOrWhiteSpace(queryDto.Name))
            {
                productEntities = await _context.Product.Include(x => x.Category).Where(x => x.Name.Contains(queryDto.Name)).ToListAsync();
            }
            else if (queryDto.CategoryGuid.HasValue && queryDto.CategoryGuid.Value != Guid.Empty)
            {
                productEntities = await _context.Product.Include(x => x.Category).Where(x => x.CategoryGuid == queryDto.CategoryGuid.Value).ToListAsync();
            }
            else
            {
                productEntities = await _context.Product.Include(x => x.Category).ToListAsync();
            }

            if (productEntities == null)
            {
                return NotFound();
            }

            var products = productEntities.Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Amount = x.Amonut,
                Price = x.Price,
                CategoryGuid = x.CategoryGuid,
                CategoryName = x.Category?.Name ?? string.Empty,
            }).OrderBy(x => x.Id);
            return Ok(products);
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductEntity([FromRoute] int id, [FromBody] ProductDto productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest();
            }

            if (!AddOrUpdateDataCheck(productDto, out string errorMessage))
            {
                return BadRequest(errorMessage);
            }

            var productEntity = new ProductEntity
            {
                Id = productDto.Id,
                Name = productDto.Name!,
                Amonut = productDto.Amount!.Value,
                Price = productDto.Price!.Value,
                CategoryGuid = productDto.CategoryGuid,
            };

            _context.Entry(productEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'BlazorCRUDDbContext.Product'  is null.");
            }

            if (!AddOrUpdateDataCheck(productDto, out string errorMessage))
            {
                return BadRequest(errorMessage);
            }

            var productEntity = new ProductEntity
            {
                Name = productDto.Name!,
                Amonut = productDto.Amount!.Value,
                Price = productDto.Price!.Value,
                CategoryGuid = productDto.CategoryGuid,
            };
            _context.Product.Add(productEntity);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProduct", new { id = productEntity.Id }, productEntity);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductEntity(int id)
        {
            if (_context.Product == null)
            {
                return NotFound();
            }
            var productEntity = await _context.Product.FindAsync(id);
            if (productEntity == null)
            {
                return NotFound();
            }

            _context.Product.Remove(productEntity);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private static bool AddOrUpdateDataCheck(ProductDto productDto, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(productDto.Name))
            {
                errorMessage = $"{nameof(productDto.Name)} cannot be empty";
            }

            if (!productDto.Amount.HasValue)
            {
                errorMessage = $"{nameof(productDto.Amount)} must have value";
            }

            if (!productDto.Price.HasValue)
            {
                errorMessage = $"{nameof(productDto.Price)} must have value";
            }

            if (productDto.CategoryGuid.HasValue && productDto.CategoryGuid.Value == Guid.Empty)
            {
                errorMessage = $"{nameof(productDto.CategoryGuid)} must have value";
            }

            return string.IsNullOrWhiteSpace(errorMessage);
        }

        private bool ProductEntityExists(int id)
        {
            return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}