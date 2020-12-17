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
    public class AdjunctTypesController : ControllerBase
    {
        private readonly bitsContext _context;

        public AdjunctTypesController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/AdjunctTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdjunctType>>> GetAdjunctType()
        {
            return await _context.AdjunctType.ToListAsync();
        }

        // GET: api/AdjunctTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdjunctType>> GetAdjunctType(int id)
        {
            var adjunctType = await _context.AdjunctType.FindAsync(id);

            if (adjunctType == null)
            {
                return NotFound();
            }

            return adjunctType;
        }

        // PUT: api/AdjunctTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdjunctType(int id, AdjunctType adjunctType)
        {
            if (id != adjunctType.AdjunctTypeId)
            {
                return BadRequest();
            }

            _context.Entry(adjunctType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdjunctTypeExists(id))
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

        // POST: api/AdjunctTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AdjunctType>> PostAdjunctType(AdjunctType adjunctType)
        {
            _context.AdjunctType.Add(adjunctType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdjunctType", new { id = adjunctType.AdjunctTypeId }, adjunctType);
        }

        // DELETE: api/AdjunctTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AdjunctType>> DeleteAdjunctType(int id)
        {
            var adjunctType = await _context.AdjunctType.FindAsync(id);
            if (adjunctType == null)
            {
                return NotFound();
            }

            _context.AdjunctType.Remove(adjunctType);
            await _context.SaveChangesAsync();

            return adjunctType;
        }

        private bool AdjunctTypeExists(int id)
        {
            return _context.AdjunctType.Any(e => e.AdjunctTypeId == id);
        }
    }
}
