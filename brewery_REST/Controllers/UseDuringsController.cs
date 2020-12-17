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
    public class UseDuringsController : ControllerBase
    {
        private readonly bitsContext _context;

        public UseDuringsController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/UseDurings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UseDuring>>> GetUseDuring()
        {
            return await _context.UseDuring.ToListAsync();
        }

        // GET: api/UseDurings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UseDuring>> GetUseDuring(int id)
        {
            var useDuring = await _context.UseDuring.FindAsync(id);

            if (useDuring == null)
            {
                return NotFound();
            }

            return useDuring;
        }

        // PUT: api/UseDurings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUseDuring(int id, UseDuring useDuring)
        {
            if (id != useDuring.UseDuringId)
            {
                return BadRequest();
            }

            _context.Entry(useDuring).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UseDuringExists(id))
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

        // POST: api/UseDurings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UseDuring>> PostUseDuring(UseDuring useDuring)
        {
            _context.UseDuring.Add(useDuring);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUseDuring", new { id = useDuring.UseDuringId }, useDuring);
        }

        // DELETE: api/UseDurings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UseDuring>> DeleteUseDuring(int id)
        {
            var useDuring = await _context.UseDuring.FindAsync(id);
            if (useDuring == null)
            {
                return NotFound();
            }

            _context.UseDuring.Remove(useDuring);
            await _context.SaveChangesAsync();

            return useDuring;
        }

        private bool UseDuringExists(int id)
        {
            return _context.UseDuring.Any(e => e.UseDuringId == id);
        }
    }
}
