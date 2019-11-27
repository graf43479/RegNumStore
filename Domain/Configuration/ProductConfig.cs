using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Domain.Entities;

namespace Domain.Configuration
{
    public class ProductConfig : EntityTypeConfiguration<Product>
    {
        public ProductConfig()
        {
            HasKey(p => p.ProductID);
            HasMany(x => x.Categories).WithMany(y => y.Products).Map(mc =>
            {
                mc.ToTable("CategoryProduct");
                mc.MapLeftKey("ProductID");
                mc.MapRightKey("CategoryID");
            }
            ); 
            //HasRequired(p => p.Categories).WithMany(x=>x.Products).HasForeignKey(m=>m.CategoryID).WillCascadeOnDelete(true);
            Property(p => p.ProductID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(p => p.Region).WithMany(x => x.Products).HasForeignKey(m => m.RegionID).WillCascadeOnDelete(false); 

            //Property(p => p.ImgExt).HasMaxLength(4).IsOptional();
            

        }
    }
}