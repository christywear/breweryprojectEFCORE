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
    public class IngredientUsedInsController : ControllerBase
    {
        private readonly bitsContext _context;

        public IngredientUsedInsController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/IngredientUsedIns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientUsedIn>>> GetIngredientUsedIn()
        {
            return await _context.IngredientUsedIn.ToListAsync();
        }

        // GET: api/IngredientUsedIns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientUsedIn>> GetIngredientUsedIn(int id)
        {
            var ingredientUsedIn = await _context.IngredientUsedIn.FindAsync(id);

            if (ingredientUsedIn == null)
            {
                return NotFound();
            }

            return ingredientUsedIn;
        }

        // PUT: api/IngredientUsedIns/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredientUsedIn(int id, IngredientUsedIn ingredientUsedIn)
        {
            if (id != ingredientUsedIn.IngredientId)
            {
                return BadRequest();
            }

            _context.Entry(ingredientUsedIn).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientUsedInExists(id))
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

        // POST: api/IngredientUsedIns
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<IngredientUsedIn>> PostIngredientUsedIn(IngredientUsedIn ingredientUsedIn)
        {
            _context.IngredientUsedIn.Add(ingredientUsedIn);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IngredientUsedInExists(ingredientUsedIn.IngredientId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetIngredientUsedIn", new { id = ingredientUsedIn.IngredientId }, ingredientUsedIn);
        }

        // DELETE: api/IngredientUsedIns/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IngredientUsedIn>> DeleteIngredientUsedIn(int id)
        {
            var ingredientUsedIn = await _context.IngredientUsedIn.FindAsync(id);
            if (ingredientUsedIn == null)
            {
                return NotFound();
            }

            _context.IngredientUsedIn.Remove(ingredientUsedIn);
            await _context.SaveChangesAsync();

            return ingredientUsedIn;
        }

        private bool IngredientUsedInExists(int id)
        {
            return _context.IngredientUsedIn.Any(e => e.IngredientId == id);
        }
    }
}
