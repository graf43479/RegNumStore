using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;

namespace RegNumStore.Models
{
    public class RegionViewModel : Region
    {
        
        //public int RegionID { get; set; }
        //public string RegionName { get; set; }
        //public string RegionNumber { get; set; }
        //public int Sequence { get; set; }
        //public bool IsActive { get; set; }
        public string RegionFullName { get; set; }

        public string RegionTypeDesc { get; set; }
        public IEnumerable<RegionType> RegionTypes{ get; set; }
        //public virtual ICollection<Product> Products { get; set; }
    }
}