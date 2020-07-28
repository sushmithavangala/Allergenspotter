using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Allergenspotter.Models
{
    public class AllergyMasterData
    {
        public int AllergyId { get; set; }

        [Key]
        public string AllergyName { get; set; }
    }
}
