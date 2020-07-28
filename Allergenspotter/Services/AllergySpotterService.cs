using Allergenspotter.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allergenspotter.Services
{
    public class AllergySpotterService : IAllergySpotterService
    {
        private readonly IAllergyDataRepository _allergyDataRepository;

        public AllergySpotterService(IAllergyDataRepository allergyDataRepository)
        {
            this._allergyDataRepository = allergyDataRepository;
        }

        public IEnumerable<String> getAllergicIngredients(String userId, List<String> ingredients)
        {
            List<String> userAllergies = _allergyDataRepository.getAllergies(userId);

            return userAllergies.Intersect(ingredients);
        }
    }
}
