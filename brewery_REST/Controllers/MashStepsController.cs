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
    public class MashStepsController : ControllerBase
    {
        private readonly bitsContext _context;

        public MashStepsController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/MashSteps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MashStep>>> GetMashStep()
        {
            return await _context.MashStep.ToListAsync();
        }

        // GET: api/MashSteps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MashStep>> GetMashStep(int id)
        {
            var mashStep = await _context.MashStep.FindAsync(id);

            if (mashStep == null)
            {
                return NotFound();
            }

            return mashStep;
        }

        // PUT: api/MashSteps/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMashStep(int id, MashStep mashStep)
        {
            if (id != mashStep.MashStepId)
            {
                return BadRequest();
            }

            _context.Entry(mashStep).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MashStepExists(id))
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

        // POST: api/MashSteps
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MashStep>> PostMashStep(MashStep mashStep)
        {
            _context.MashStep.Add(mashStep);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMashStep", new { id = mashStep.MashStepId }, mashStep);
        }

        // DELETE: api/MashSteps/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MashStep>> DeleteMashStep(int id)
        {
            var mashStep = await _context.MashStep.FindAsync(id);
            if (mashStep == null)
            {
                return NotFound();
            }

            _context.MashStep.Remove(mashStep);
            await _context.SaveChangesAsync();

            return mashStep;
        }

        private bool MashStepExists(int id)
        {
            return _context.MashStep.Any(e => e.MashStepId == id);
        }
    }
}
