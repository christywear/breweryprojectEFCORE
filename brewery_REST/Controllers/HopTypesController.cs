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
    public class HopTypesController : ControllerBase
    {
        private readonly bitsContext _context;

        public HopTypesController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/HopTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HopType>>> GetHopType()
        {
            return await _context.HopType.ToListAsync();
        }

        // GET: api/HopTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HopType>> GetHopType(int id)
        {
            var hopType = await _context.HopType.FindAsync(id);

            if (hopType == null)
            {
                return NotFound();
            }

            return hopType;
        }

        // PUT: api/HopTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHopType(int id, HopType hopType)
        {
            if (id != hopType.HopTypeId)
            {
                return BadRequest();
            }

            _context.Entry(hopType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HopTypeExists(id))
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

        // POST: api/HopTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<HopType>> PostHopType(HopType hopType)
        {
            _context.HopType.Add(hopType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHopType", new { id = hopType.HopTypeId }, hopType);
        }

        // DELETE: api/HopTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HopType>> DeleteHopType(int id)
        {
            var hopType = await _context.HopType.FindAsync(id);
            if (hopType == null)
            {
                return NotFound();
            }

            _context.HopType.Remove(hopType);
            await _context.SaveChangesAsync();

            return hopType;
        }

        private bool HopTypeExists(int id)
        {
            return _context.HopType.Any(e => e.HopTypeId == id);
        }
    }
}
