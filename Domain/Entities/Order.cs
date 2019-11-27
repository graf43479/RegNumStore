using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
      //  [Key]
        public int OrderID { get; set; }
        //  public int CategoryID { get; set; }
        public DateTime StartDate { get; set; }
        public string ProductNumber { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }

        public int RegionID { get; set; }

        public string Comment { get; set; }

        public bool IsForSale { get; set; }
    }
}
