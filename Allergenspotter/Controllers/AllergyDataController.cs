using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Allergenspotter.Models;
using Allergenspotter.Services;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace Allergenspotter.Controllers
{
    [Route("api/AllergyData")]
    [ApiController]
    public class AllergyDataController : ControllerBase
    {
        private readonly AllergyContext _context;

        private readonly IAllergySpotterService allergySpotterService;

        private readonly ComputerVisionClient _client;
        private readonly ComputerVisionService _cvService;

        public AllergyDataController(AllergyContext context, IAllergySpotterService allergySpotterService, ComputerVisionClient client, ComputerVisionService cvService)
        {
            _context = context;
            this.allergySpotterService = allergySpotterService;
            _cvService = cvService;
            _client = client;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllergyData>>> GetAllergyData()
        {
            return await _context.AllergyData.ToListAsync();
        }

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

        [HttpPost("cvTest/{userId}")]
        public async Task<ActionResult<IEnumerable<String>>> GetOcrResult(String userId, [FromBody] ComputerVisionRequest cvRequest)
        {
            var ingredients = await _cvService.BatchReadFileUrl(_client,cvRequest.ImageUrl);
            return ingredients;
        }

        [HttpPost("{userId}")]
        // public async Task<ActionResult<IEnumerable<String>>> GetUserAllergyData(String userId, [FromBody] IngredientsData ingredientsData)
        public async Task<ActionResult<IEnumerable<String>>> GetUserAllergyData(String userId, [FromBody] ComputerVisionRequest cvRequest)
        {
            var ingredients = await _cvService.BatchReadFileUrl(_client,cvRequest.ImageUrl);
            var allergyData =  allergySpotterService.getAllergicIngredients(userId, ingredients);
            // var allergyData =  allergySpotterService.getAllergicIngredients(userId, ingredientsData.ingredients);

            if (allergyData == null)
            {
                return NotFound();
            }

            return Ok(allergyData);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAllergyData(AllergyData allergyData)
        {
            if (allergyData.UserId == null)
            {
                return BadRequest("Error : user id mandatory");
            }

            var existingAllergyData = await _context.AllergyData.FindAsync(allergyData.UserId);
            if (existingAllergyData == null)
            {
                return BadRequest("Error : No user found with the user id used");
            }
            updateAllergyData(allergyData, existingAllergyData);

            _context.Entry(existingAllergyData).State = EntityState.Modified;
            _context.Entry(existingAllergyData).Property(x => x.Id).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AllergyDataExists(allergyData.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(existingAllergyData);
        }

        private static void updateAllergyData (AllergyData allergyData, AllergyData existingAllergyData)
        {
            if (allergyData.UserName != null)
            {
                existingAllergyData.UserName = allergyData.UserName;
            }

            if (allergyData.Allergy1 != null)
            {
                existingAllergyData.Allergy1 = allergyData.Allergy1;
            }

            if (allergyData.Allergy2 != null)
            {
                existingAllergyData.Allergy2 = allergyData.Allergy2;
            }

            if (allergyData.Allergy3 != null)
            {
                existingAllergyData.Allergy2 = allergyData.Allergy2;
            }

            if (allergyData.Allergy4 != null)
            {
                existingAllergyData.Allergy2 = allergyData.Allergy2;
            }

            if (allergyData.Allergy5 != null)
            {
                existingAllergyData.Allergy2 = allergyData.Allergy2;
            }
        }

        [HttpPost]
        public async Task<ActionResult<AllergyData>> PostAllergyData(AllergyData allergyData)
        {
            _context.AllergyData.Add(allergyData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAllergyData", new { userId = allergyData.UserId }, allergyData);
        }


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
