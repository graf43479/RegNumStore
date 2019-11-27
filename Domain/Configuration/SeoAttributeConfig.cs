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
    public class SeoAttributeConfig : EntityTypeConfiguration<SeoAttribute>
    {
        public SeoAttributeConfig()
        {
            HasKey(p => p.TagID);
            Property(p => p.TagID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.CreateDate).HasColumnType("datetime2");
            Property(p => p.UpdateDate).HasColumnType("datetime2");
            Map(m =>
            {
                m.Properties(d => new
                {
                    d.TagID,
                    d.CreateDate,
                    d.UpdateDate
                });
                m.ToTable("SeoArticleBase");
            })
                .Map(m =>
                {
                    m.Properties(d => new
                    {
                        d.TagID,
                        d.Keywords,
                        d.Snippet,
                        d.Tittle,
                        d.Robots
                    });
                    m.ToTable("Seo");
                })
                .Map(m =>
                {
                    m.Properties(d => new
                    {
                        d.TagID,
                        d.Tag,
                        d.Header,
                        d.ArticlePreview,
                        d.ArticleText,
                        d.ShortLink,
                        d.UserID
                    });
                    m.ToTable("ArticleContent");
                });
        }
    }
}
