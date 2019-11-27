using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IRegionRepository
    {
        IQueryable<Region> Regions { get; }

        void SaveRegion(Region region);

        void DeleteRegion(Region region);
    }
}
