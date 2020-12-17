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
    public class YeastsController : ControllerBase
    {
        private readonly bitsContext _context;

        public YeastsController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/Yeasts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Yeast>>> GetYeast()
        {
            return await _context.Yeast.ToListAsync();
        }

        // GET: api/Yeasts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Yeast>> GetYeast(int id)
        {
            var yeast = await _context.Yeast.FindAsync(id);

            if (yeast == null)
            {
                return NotFound();
            }

            return yeast;
        }

        // PUT: api/Yeasts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutYeast(int id, Yeast yeast)
        {
            if (id != yeast.IngredientId)
            {
                return BadRequest();
            }

            _context.Entry(yeast).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!YeastExists(id))
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

        // POST: api/Yeasts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Yeast>> PostYeast(Yeast yeast)
        {
            _context.Yeast.Add(yeast);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (YeastExists(yeast.IngredientId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetYeast", new { id = yeast.IngredientId }, yeast);
        }

        // DELETE: api/Yeasts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Yeast>> DeleteYeast(int id)
        {
            var yeast = await _context.Yeast.FindAsync(id);
            if (yeast == null)
            {
                return NotFound();
            }

            _context.Yeast.Remove(yeast);
            await _context.SaveChangesAsync();

            return yeast;
        }

        private bool YeastExists(int id)
        {
            return _context.Yeast.Any(e => e.IngredientId == id);
        }
    }
}
