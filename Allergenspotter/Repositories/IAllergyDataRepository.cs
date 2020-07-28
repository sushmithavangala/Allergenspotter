using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allergenspotter.Repositories
{
    public interface IAllergyDataRepository
    {
        public List<String> getAllergies(String userId);
    }
}
