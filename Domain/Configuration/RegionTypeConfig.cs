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
    public class RegionTypeConfig : EntityTypeConfiguration<RegionType>
    {
        public RegionTypeConfig()
        {
            HasKey(p => p.RegionTypeID);
           // HasRequired(p => p.Products);
            Property(p => p.RegionTypeID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasMany(x => x.Regions).WithRequired(x => x.RegionType).HasForeignKey(x => x.RegionTypeID).WillCascadeOnDelete(false);
        }
    }
}
