using Allergenspotter.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allergenspotter.Repositories
{
    public class AllergyDataRepository : IAllergyDataRepository
    {
        private readonly AllergyContext _allergyContext;

        public AllergyDataRepository(AllergyContext dbContext)
        {
            _allergyContext = dbContext;
        }

        public List<String> getAllergies(String userId)
        {
            var db = _allergyContext;
            AllergyData allergyData = db.AllergyData.Find(userId);
            if (allergyData == null)
            {
                return new List<String>();
                //:TODO throw an exception
            }
            List<String> allergyList = new List<String>();
            allergyList.Add(allergyData.Allergy1);
            allergyList.Add(allergyData.Allergy2);
            allergyList.Add(allergyData.Allergy3);
            allergyList.Add(allergyData.Allergy4);
            allergyList.Add(allergyData.Allergy5);

            return allergyList;
        }
    }
}
