using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Domain.Configuration;
using Domain.Entities;

namespace Domain.Concrete
{
    public class RegNumDBContext : DbContext
    {
        public RegNumDBContext() //: base("RegNumDBContext") 
        {
           // Database.SetInitializer<RegNumDBContext>(new DropCreateDatabaseAlways<RegNumDBContext>());
            
           //Database.SetInitializer(new RegnumDBInitializer());
            //поставить null на продакшене
             Database.SetInitializer<RegNumDBContext>(null);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<RegionType> RegionTypes { get; set; }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Article> Articles { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<SeoAttribute> SeoAttributes { get; set; }
        public DbSet<MailSettings> MailSettingses { get; set; }


        //public DbSet<SeoArticleBase> SeoArticleBases { get; set; }
        //public DbSet<Seo> Seos { get; set; }
        //public DbSet<ArticleDerived> ArticleDeriveds { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new ProductConfig());
            modelBuilder.Configurations.Add(new CategoryConfig());
            modelBuilder.Configurations.Add(new UserConfig());
            modelBuilder.Configurations.Add(new RoleConfig());
            modelBuilder.Configurations.Add(new CommentConfig());
            modelBuilder.Configurations.Add(new RegionConfig());
            modelBuilder.Configurations.Add(new ArticleConfig());
            modelBuilder.Configurations.Add(new OrderConfig());
            modelBuilder.Configurations.Add(new SeoAttributeConfig());
            modelBuilder.Configurations.Add(new MailSettingsConfig());

            //modelBuilder.Configurations.Add(new SeoArticleBaseConfig());
            //modelBuilder.Configurations.Add(new SeoConfig());
            //modelBuilder.Configurations.Add(new ArticleDerivedConfig());
        }
    }
}