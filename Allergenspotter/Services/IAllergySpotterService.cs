using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allergenspotter.Services
{
    public interface IAllergySpotterService
    {
        public IEnumerable<String> getAllergicIngredients(String userId, List<String> ingredients);
    }
}
