using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Allergenspotter.Models
{
    public class AllergyMasterData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AllergyId { get; set; }

        [Key]
        public string AllergyName { get; set; }
    }
}
