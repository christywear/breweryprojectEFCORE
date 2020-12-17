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
    public class MashesController : ControllerBase
    {
        private readonly bitsContext _context;

        public MashesController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/Mashes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mash>>> GetMash()
        {
            return await _context.Mash.ToListAsync();
        }

        // GET: api/Mashes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mash>> GetMash(int id)
        {
            var mash = await _context.Mash.FindAsync(id);

            if (mash == null)
            {
                return NotFound();
            }

            return mash;
        }

        // PUT: api/Mashes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMash(int id, Mash mash)
        {
            if (id != mash.MashId)
            {
                return BadRequest();
            }

            _context.Entry(mash).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MashExists(id))
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

        // POST: api/Mashes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Mash>> PostMash(Mash mash)
        {
            _context.Mash.Add(mash);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMash", new { id = mash.MashId }, mash);
        }

        // DELETE: api/Mashes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Mash>> DeleteMash(int id)
        {
            var mash = await _context.Mash.FindAsync(id);
            if (mash == null)
            {
                return NotFound();
            }

            _context.Mash.Remove(mash);
            await _context.SaveChangesAsync();

            return mash;
        }

        private bool MashExists(int id)
        {
            return _context.Mash.Any(e => e.MashId == id);
        }
    }
}
