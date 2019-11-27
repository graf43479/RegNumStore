using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Domain.Entities;

namespace RegNumStore.Models
{
    public class ProductRegNumViewModel: CarNumber
    {
        //public CarNumber Carnumber { get; set; }
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        //public int CategoryID { get; set; }
        //public int UserID { get; set; }
        //public int RegionID { get; set; }
        [Required]
        [Display(Name = "Цена")]
        //[DataType(DataType.Currency)]
        [Range(0, 3000000)]
        public decimal Price { get; set; }
        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Первес номера")]
        public bool IsOverbalanceIncluded { get; set; }
        
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Region> Regions { get; set; }
    }
}