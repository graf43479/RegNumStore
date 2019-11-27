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
   public class EFRegionTypeRepository : IRegionTypeRepository
    {
        private RegNumDBContext context;

        public EFRegionTypeRepository(RegNumDBContext context)
        {
            this.context = context;
        }

         public IQueryable<RegionType> RegionTypes
         {
             get { return context.RegionTypes; } 
            }

         public void SaveRegionType(RegionType regionType)
        {
            if (regionType.RegionTypeID== 0)
            {
                context.RegionTypes.Add(regionType);
            }
            else
            {
                context.Entry(regionType).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

         public void DeleteRegionType(RegionType regionType)
        {
            context.RegionTypes.Remove(regionType);
            context.SaveChanges();
        }
    }
}
