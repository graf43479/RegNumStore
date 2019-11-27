using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Configuration
{
    public class RegionConfig : EntityTypeConfiguration<Region>
    {
        public RegionConfig()
        {
            HasKey(p => p.RegionID);
           // HasRequired(p => p.Products);
            Property(p => p.RegionID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasMany(x => x.Products).WithRequired(x => x.Region).HasForeignKey(x => x.RegionID).WillCascadeOnDelete(false);
        }
    }
}
