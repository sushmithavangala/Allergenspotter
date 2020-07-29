using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Allergenspotter.Models
{
    public class AllergyData
    {
        [Key]
        public String UserId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public String  Allergy1 { get; set; }
        public String Allergy2 { get; set; }
        public String Allergy3 { get; set; }
        public String Allergy4 { get; set; }
        public String Allergy5 { get; set; }
    }
}
