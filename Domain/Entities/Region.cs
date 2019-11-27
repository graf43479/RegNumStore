using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
   public class Region
    {
       public Region()
       {
           RegionTypeID = 1;
       }

    //   [Key]
        public int RegionID { get; set; }

       public int RegionTypeID { get; set; }

       public string RegionName { get; set; }
        public string RegionNumber { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual RegionType RegionType { get; set; }
    }
}
