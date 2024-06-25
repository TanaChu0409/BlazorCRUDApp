using BlazorCRUDApp.Api.Entities;
using BlazorCRUDApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorCRUDApp.Api.Controllers
{
    [Route("api/sub-category")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly BlazorCRUDDbContext _context;

        public SubCategoryController(BlazorCRUDDbContext context)
        {
            _context = context;
        }

        // GET: api/SubCategory
        [HttpGet]
        public async Task<IActionResult> GetSubCategory()
        {
            if (_context.SubCategory == null)
            {
                return NotFound();
            }
            var subCategoriesEntity = await _context.SubCategory.Include(x => x.Category).ToListAsync();
            var subCategoriesDto = subCategoriesEntity.Select(x => new SubCategoryDto
            {
                Id = x.Id,
                Uuid = x.Uuid,
                Name = x.Name,
                Description = x.Description,
                LastUpdateDate = x.LastUpdateDate,
                CategoryName = x.Category.Name,
                CategoryUid = x.CategoryUid,
            }).OrderBy(x => x.Id);
            return Ok(subCategoriesDto);
        }

        // GET: api/SubCategory/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubCategoryEntity([FromRoute] int id)
        {
            if (_context.SubCategory == null)
            {
                return NotFound();
            }
            var subCategoryEntity = await _context.SubCategory.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

            if (subCategoryEntity == null)
            {
                return NotFound();
            }

            var subCategoryDto = new SubCategoryDto
            {
                Id = subCategoryEntity.Id,
                Uuid = subCategoryEntity.Uuid,
                Name = subCategoryEntity.Name,
                Description = subCategoryEntity.Description,
                CategoryUid = subCategoryEntity.CategoryUid,
                CategoryName = subCategoryEntity.Category.Name,
                LastUpdateDate = subCategoryEntity.LastUpdateDate,
            };
            return Ok(subCategoryDto);
        }

        [HttpGet("query")]
        public async Task<IActionResult> GetSubCateory([FromQuery] SubCategoryQueryDto queryDto)
        {
            if (_context.SubCategory == null)
            {
                return NotFound();
            }

            var subCategoriesEntity = new List<SubCategoryEntity>();
            if (!string.IsNullOrWhiteSpace(queryDto.Name) && queryDto.CategoryUid.HasValue && queryDto.CategoryUid.Value != Guid.Empty)
            {
                subCategoriesEntity = await _context.SubCategory.Include(x => x.Category).Where(x => x.Name.Contains(queryDto.Name) && x.CategoryUid == queryDto.CategoryUid.Value).ToListAsync();
            }
            else if (!string.IsNullOrWhiteSpace(queryDto.Name))
            {
                subCategoriesEntity = await _context.SubCategory.Include(x => x.Category).Where(x => x.Name.Contains(queryDto.Name)).ToListAsync();
            }
            else if (queryDto.CategoryUid.HasValue && queryDto.CategoryUid.Value != Guid.Empty)
            {
                subCategoriesEntity = await _context.SubCategory.Include(x => x.Category).Where(x => x.CategoryUid == queryDto.CategoryUid.Value).ToListAsync();
            }
            else
            {
                subCategoriesEntity = await _context.SubCategory.Include(x => x.Category).ToListAsync();
            }

            var subCategoriesDto = subCategoriesEntity.Select(x => new SubCategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                CategoryUid = x.CategoryUid,
                CategoryName = x.Category.Name,
                Description = x.Description,
                Uuid = x.CategoryUid,
                LastUpdateDate = x.LastUpdateDate,
            }).OrderBy(x => x.Id);
            return Ok(subCategoriesDto);
        }

        // PUT: api/SubCategory/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubCategoryEntity([FromRoute] int id, [FromBody] SubCategoryDto subCategoryDto)
        {
            if (id != subCategoryDto.Id)
            {
                return BadRequest();
            }

            if (!CheckDataIsValidated(subCategoryDto, out string errorMessage))
            {
                return BadRequest(errorMessage);
            }

            var subCategoryEntity = new SubCategoryEntity
            {
                Id = subCategoryDto.Id,
                CategoryUid = subCategoryDto.CategoryUid!.Value,
                Name = subCategoryDto.Name!,
                Description = subCategoryDto.Description,
            };

            _context.Entry(subCategoryEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubCategoryEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SubCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostSubCategoryEntity([FromBody] SubCategoryDto subCategoryDto)
        {
            if (_context.SubCategory == null)
            {
                return Problem("Entity set 'BlazorCRUDDbContext.SubCategory'  is null.");
            }

            if (!CheckDataIsValidated(subCategoryDto, out string errorMessage))
            {
                return BadRequest(errorMessage);
            }

            var subCategoryEntity = new SubCategoryEntity
            {
                CategoryUid = subCategoryDto.CategoryUid!.Value,
                Name = subCategoryDto.Name!,
                Description = subCategoryDto.Description,
            };

            _context.SubCategory.Add(subCategoryEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubCategoryEntity", new { id = subCategoryEntity.Id }, subCategoryEntity);
        }

        // DELETE: api/SubCategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubCategoryEntity([FromRoute] int id)
        {
            if (_context.SubCategory == null)
            {
                return NotFound();
            }
            var subCategoryEntity = await _context.SubCategory.FindAsync(id);
            if (subCategoryEntity == null)
            {
                return NotFound();
            }

            _context.SubCategory.Remove(subCategoryEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static bool CheckDataIsValidated(SubCategoryDto subCategoryDto, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!subCategoryDto.CategoryUid.HasValue)
            {
                errorMessage = $"{nameof(subCategoryDto.CategoryUid)} must have a value";
            }
            else if (subCategoryDto.CategoryUid.Value == Guid.Empty)
            {
                errorMessage = $"{nameof(subCategoryDto.CategoryUid)} cannot be empty";
            }
            else if (string.IsNullOrWhiteSpace(subCategoryDto.Name))
            {
                errorMessage = $"{nameof(subCategoryDto.Name)} must have a value";
            }
            return string.IsNullOrWhiteSpace(errorMessage);
        }

        private bool SubCategoryEntityExists(int id)
        {
            return (_context.SubCategory?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}