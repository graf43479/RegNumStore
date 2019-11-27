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
    public class ArticleConfig : EntityTypeConfiguration<Article>
    {
        public ArticleConfig()
        {
            HasKey(p => p.ArticleID);
            HasRequired(p => p.User);
            Property(p => p.ArticleID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.UpdateDate).HasColumnType("datetime2");
            Property(p => p.CreateDate).HasColumnType("datetime2");
        }
    }
}
