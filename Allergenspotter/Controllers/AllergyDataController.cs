using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Allergenspotter.Models;
using Allergenspotter.Services;

namespace Allergenspotter.Controllers
{
    [Route("api/AllergyData")]
    [ApiController]
    public class AllergyDataController : ControllerBase
    {
        private readonly AllergyContext _context;

        private readonly IAllergySpotterService allergySpotterService;

        public AllergyDataController(AllergyContext context, IAllergySpotterService allergySpotterService)
        {
            _context = context;
            this.allergySpotterService = allergySpotterService;
        }

        // GET: api/AllergyData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllergyData>>> GetAllergyData()
        {
            return await _context.AllergyData.ToListAsync();
        }

        // GET: api/AllergyData/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<AllergyData>> GetAllergyData([System.Web.Http.FromUri] String userId)
        {
            var allergyData = await _context.AllergyData.FindAsync(userId);

            if (allergyData == null)
            {
                return NotFound();
            }

            return allergyData;
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<IEnumerable<String>>> GetUserAllergyData(String userId, [FromBody] IngredientsData ingredientsData)
        {
            var allergyData =  allergySpotterService.getAllergicIngredients(userId, ingredientsData.ingredients);

            if (allergyData == null)
            {
                return NotFound();
            }

            return Ok(allergyData);
        }

        // PUT: api/AllergyData/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutAllergyData(String  userId, AllergyData allergyData)
        {
            if (!userId.Equals(allergyData.UserId))
            {
                return BadRequest();
            }

            _context.Entry(allergyData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AllergyDataExists(userId))
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

        // POST: api/AllergyData
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /*[HttpPost]
        public async Task<ActionResult<AllergyData>> PostAllergyData(AllergyData allergyData)
        {
            _context.AllergyData.Add(allergyData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAllergyData", new { userId = allergyData.UserId }, allergyData);
        }*/

        // DELETE: api/AllergyData/5
        [HttpDelete("{userId}")]
        public async Task<ActionResult<AllergyData>> DeleteAllergyData(String userId)
        {
            var allergyData = await _context.AllergyData.FindAsync(userId);
            if (allergyData == null)
            {
                return NotFound();
            }

            _context.AllergyData.Remove(allergyData);
            await _context.SaveChangesAsync();

            return allergyData;
        }

        private bool AllergyDataExists(String userId)
        {
            return _context.AllergyData.Any(e => e.UserId == userId);
        }
    }
}
