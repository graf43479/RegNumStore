using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RegionType
    {
    //    [Key]
        public int RegionTypeID { get; set; }

        public string RegionTypeDesc { get; set; }
        public virtual ICollection<Region> Regions { get; set; }
    }
}
