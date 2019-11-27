using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Domain.Entities
{
    public class Product
    {
     //   [Key]
        public int ProductID { get; set; }
      //  public int CategoryID { get; set; }
        public int UserID { get; set; }
        public int RegionID { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ProductNumber { get; set; }
        public string Status { get; set; }
        //public int Sequence { get; set; }
        //public string ImgExt { get; set; }
        //public string Path { get; set; }
        public bool IsForSale { get; set; }
        public bool IsDisplay { get; set; }
        public bool IsChoosen { get; set; }

        public bool IsOverbalanceIncluded { get; set; }
        //public virtual Category Category { get; set; }
        
        public Region Region { get; set; }
        public User User { get; set; }

        //public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Category> Categories { get; set; }

        public Product()
        {
            Categories= new List<Category>();
        }
    }
}