using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFSeoAttributeRepository : ISeoAttributeRepository
    {
        private RegNumDBContext context;

        public EFSeoAttributeRepository(RegNumDBContext context)
        {
            this.context = context;
        }

        public IQueryable<SeoAttribute> SeoAttributes
        {
            get { return context.SeoAttributes; }
        }
        

        public void SaveSeoAttributes(SeoAttribute seoAttribute)
        {
            if (!context.SeoAttributes.Any(x => x.TagID==seoAttribute.TagID))
            {
                context.SeoAttributes.Add(seoAttribute);
            }
            else
            {
                context.Entry(seoAttribute).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteSeoAttributes(SeoAttribute seoAttribute)
        {
            context.SeoAttributes.Remove(seoAttribute);
            context.SaveChanges();
        }

    }
}
