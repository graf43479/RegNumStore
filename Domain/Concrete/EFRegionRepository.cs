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
   public class EFRegionRepository : IRegionRepository
    {
        private RegNumDBContext context;

        public EFRegionRepository(RegNumDBContext context)
        {
            this.context = context;
        }

         public IQueryable<Region> Regions
         {
             get { return context.Regions; } 
            }

         public void SaveRegion(Region region)
        {
            if (region.RegionID == 0)
            {
                context.Regions.Add(region);
            }
            else
            {
                context.Entry(region).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

         public void DeleteRegion(Region region)
        {
            context.Regions.Remove(region);
            context.SaveChanges();
        }
    }
}
