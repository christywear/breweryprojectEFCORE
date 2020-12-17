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
    public class IngredientSubstitutesController : ControllerBase
    {
        private readonly bitsContext _context;

        public IngredientSubstitutesController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/IngredientSubstitutes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientSubstitute>>> GetIngredientSubstitute()
        {
            return await _context.IngredientSubstitute.ToListAsync();
        }

        // GET: api/IngredientSubstitutes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientSubstitute>> GetIngredientSubstitute(int id)
        {
            var ingredientSubstitute = await _context.IngredientSubstitute.FindAsync(id);

            if (ingredientSubstitute == null)
            {
                return NotFound();
            }

            return ingredientSubstitute;
        }

        // PUT: api/IngredientSubstitutes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredientSubstitute(int id, IngredientSubstitute ingredientSubstitute)
        {
            if (id != ingredientSubstitute.IngredientId)
            {
                return BadRequest();
            }

            _context.Entry(ingredientSubstitute).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientSubstituteExists(id))
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

        // POST: api/IngredientSubstitutes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<IngredientSubstitute>> PostIngredientSubstitute(IngredientSubstitute ingredientSubstitute)
        {
            _context.IngredientSubstitute.Add(ingredientSubstitute);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IngredientSubstituteExists(ingredientSubstitute.IngredientId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetIngredientSubstitute", new { id = ingredientSubstitute.IngredientId }, ingredientSubstitute);
        }

        // DELETE: api/IngredientSubstitutes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IngredientSubstitute>> DeleteIngredientSubstitute(int id)
        {
            var ingredientSubstitute = await _context.IngredientSubstitute.FindAsync(id);
            if (ingredientSubstitute == null)
            {
                return NotFound();
            }

            _context.IngredientSubstitute.Remove(ingredientSubstitute);
            await _context.SaveChangesAsync();

            return ingredientSubstitute;
        }

        private bool IngredientSubstituteExists(int id)
        {
            return _context.IngredientSubstitute.Any(e => e.IngredientId == id);
        }
    }
}
