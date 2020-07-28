using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Allergenspotter.Models;

namespace Allergenspotter.Controllers
{
    [Route("api/AllergyMasterData")]
    [ApiController]
    public class AllergyMasterDataController : ControllerBase
    {
        private readonly AllergyContext _context;

        public AllergyMasterDataController(AllergyContext context)
        {
            _context = context;
        }

        // GET: api/AllergyMasterData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllergyMasterData>>> GetAllergyMasterData()
        {
            return await _context.AllergyMasterData.ToListAsync();
        }

        // GET: api/AllergyMasterData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AllergyMasterData>> GetAllergyMasterData(string id)
        {
            var allergyMasterData = await _context.AllergyMasterData.FindAsync(id);

            if (allergyMasterData == null)
            {
                return NotFound();
            }

            return allergyMasterData;
        }

        // PUT: api/AllergyMasterData/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAllergyMasterData(string id, AllergyMasterData allergyMasterData)
        {
            if (id != allergyMasterData.AllergyName)
            {
                return BadRequest();
            }

            _context.Entry(allergyMasterData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AllergyMasterDataExists(id))
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

        // POST: api/AllergyMasterData
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AllergyMasterData>> PostAllergyMasterData(AllergyMasterData allergyMasterData)
        {
            _context.AllergyMasterData.Add(allergyMasterData);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AllergyMasterDataExists(allergyMasterData.AllergyName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAllergyMasterData", new { id = allergyMasterData.AllergyName }, allergyMasterData);
        }

        // DELETE: api/AllergyMasterData/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AllergyMasterData>> DeleteAllergyMasterData(string id)
        {
            var allergyMasterData = await _context.AllergyMasterData.FindAsync(id);
            if (allergyMasterData == null)
            {
                return NotFound();
            }

            _context.AllergyMasterData.Remove(allergyMasterData);
            await _context.SaveChangesAsync();

            return allergyMasterData;
        }

        private bool AllergyMasterDataExists(string id)
        {
            return _context.AllergyMasterData.Any(e => e.AllergyName == id);
        }
    }
}
