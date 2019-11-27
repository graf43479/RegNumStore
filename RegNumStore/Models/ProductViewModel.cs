using System;
using System.Collections.Generic;
using Domain.Entities;

namespace RegnumStore.Models
{
    public class ProductViewModel
    {

        public int ProductID { get; set; }
      //  public int SelectedCategoryID { get; set; }
        public int UserID { get; set; }
        public int SelectedRegionID { get; set; }
        public decimal Price { get; set; }
        public string ProductNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Status { get; set; }
        public bool IsForSale { get; set; }
        public bool IsOverbalanceIncluded { get; set; }
        public bool IsDisplay { get; set; }
        public bool IsChoosen { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Region> Regions { get; set; }
        public string CategoryName { get; set; }
        public string RegionNumber { get; set; }
        public string RegionName { get; set; }
        public Region Region { get; set; }
        public User User { get; set; }
    }
}
 
        