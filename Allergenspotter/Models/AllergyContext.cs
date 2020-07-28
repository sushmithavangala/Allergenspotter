using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Allergenspotter.Models
{
    public class AllergyContext : DbContext
    {
        public AllergyContext(DbContextOptions<AllergyContext> options)
           : base(options)
        {
        }

        public DbSet<AllergyData> AllergyData { get; set; }

        public DbSet<AllergyMasterData> AllergyMasterData { get; set; }

    }
}
