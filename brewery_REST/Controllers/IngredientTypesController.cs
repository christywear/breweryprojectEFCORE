using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreweryEFCoreClasses.Models;

namespace brewery_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientTypesController : ControllerBase
    {
        private readonly bitsContext _context;

        public IngredientTypesController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/IngredientTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientType>>> GetIngredientType()
        {
            return await _context.IngredientType.ToListAsync();
        }

        // GET: api/IngredientTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientType>> GetIngredientType(int id)
        {
            var ingredientType = await _context.IngredientType.FindAsync(id);

            if (ingredientType == null)
            {
                return NotFound();
            }

            return ingredientType;
        }

        // PUT: api/IngredientTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredientType(int id, IngredientType ingredientType)
        {
            if (id != ingredientType.IngredientTypeId)
            {
                return BadRequest();
            }

            _context.Entry(ingredientType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientTypeExists(id))
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

        // POST: api/IngredientTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<IngredientType>> PostIngredientType(IngredientType ingredientType)
        {
            _context.IngredientType.Add(ingredientType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIngredientType", new { id = ingredientType.IngredientTypeId }, ingredientType);
        }

        // DELETE: api/IngredientTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IngredientType>> DeleteIngredientType(int id)
        {
            var ingredientType = await _context.IngredientType.FindAsync(id);
            if (ingredientType == null)
            {
                return NotFound();
            }

            _context.IngredientType.Remove(ingredientType);
            await _context.SaveChangesAsync();

            return ingredientType;
        }

        private bool IngredientTypeExists(int id)
        {
            return _context.IngredientType.Any(e => e.IngredientTypeId == id);
        }
    }
}
