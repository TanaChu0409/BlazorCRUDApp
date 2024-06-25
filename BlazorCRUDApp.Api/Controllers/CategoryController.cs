using BlazorCRUDApp.Api.Entities;
using BlazorCRUDApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorCRUDApp.Api.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly BlazorCRUDDbContext _context;

        public CategoryController(BlazorCRUDDbContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            if (_context.Category == null)
            {
                return NotFound();
            }

            var categoriesEntity = await _context.Category.ToListAsync();
            var categories = categoriesEntity.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Uuid = x.Uuid,
                Description = x.Description,
            }).OrderBy(x => x.Id);

            return Ok(categories);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory([FromRoute] int id)
        {
            if (_context.Category == null)
            {
                return NotFound();
            }
            var categoryEntitiy = await _context.Category.FindAsync(id);

            if (categoryEntitiy == null)
            {
                return NotFound();
            }

            var category = new CategoryDto
            {
                Id = categoryEntitiy.Id,
                Name = categoryEntitiy.Name,
                Description = categoryEntitiy.Description,
            };

            return Ok(category);
        }

        [HttpGet("name")]
        public async Task<IActionResult> GetCategories([FromQuery] CategoryQueryDto queryDto)
        {
            if (_context.Category == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(queryDto.Name))
            {
                return BadRequest();
            }

            var categoriesEntity = await _context.Category.Where(x => x.Name.Contains(queryDto.Name)).ToListAsync();
            if (categoriesEntity == null)
            {
                return NotFound();
            }
            var categories = categoriesEntity.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }).OrderBy(x => x.Id);

            return Ok(categories);
        }

        // PUT: api/Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryEntitiy([FromRoute] int id, [FromBody] CategoryDto categoryDto)
        {
            if (id != categoryDto.Id)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(categoryDto.Name))
            {
                return BadRequest($"{nameof(categoryDto.Name)} cannot be empty");
            }

            var categoryEntity = new CategoryEntity
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                Description = categoryDto.Description,
            };

            _context.Entry(categoryEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryEntitiyExists(id))
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

        // POST: api/Category
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostCategoryEntitiy([FromBody] CategoryDto categoryDto)
        {
            if (_context.Category == null)
            {
                return Problem("Entity set 'BlazorCRUDDbContext.Category'  is null.");
            }

            if (string.IsNullOrWhiteSpace(categoryDto.Name))
            {
                return BadRequest($"{nameof(categoryDto.Name)} cannot be empty");
            }

            var categoryEntity = new CategoryEntity
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
            };

            _context.Category.Add(categoryEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = categoryEntity.Id }, categoryEntity);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryEntitiy([FromRoute] int id)
        {
            if (_context.Category == null)
            {
                return NotFound();
            }
            var categoryEntitiy = await _context.Category.FindAsync(id);
            if (categoryEntitiy == null)
            {
                return NotFound();
            }

            _context.Category.Remove(categoryEntitiy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryEntitiyExists(int id)
        {
            return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}