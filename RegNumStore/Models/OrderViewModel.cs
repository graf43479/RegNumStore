using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Domain;
using Domain.Entities;

namespace RegNumStore.Models
{
    public class OrderViewModel : CarNumber
    {
        public int OrderID { get; set; }
        //  public int CategoryID { get; set; }
        public DateTime StartDate { get; set; }

       // [Required]
        [Display(Name = "Номер")]
        public string ProductNumber
        {
            get { return Num1 + Num2 + Num3 + Num4 + Num5 + Num6 + Num7; }
            set { value = Num1 + Num2 + Num3 + Num4 + Num5 + Num6 + Num7 ; } 
        }
       
        [Required(ErrorMessage = "Необходимо заполнить поле Email")]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9.-]{1,20}@[a-zA-Z0-9.-]{1,20}\.[A-Za-z]{2,4}", ErrorMessage = "Неверный формат Email")]
        public string Email { get; set; }

        [Display(Name = "Телефон")]
        //[DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Необходимо заполнить поле  \"Телефон\"")]
        public string Phone { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Необходимо заполнить поле Имя")]
        [StringLength(Constants.LOGIN_MAX_LENGTH, MinimumLength = Constants.LOGIN_MIN_LENGTH, ErrorMessage = "Длина имени должна быть в диапазоне 3-20 символов")]
        public string Name { get; set; }

        [Display(Name = "Комментарий")]
        [StringLength(140, ErrorMessage = "Длина комментария должна быть в до 140 символов")]
        public string Comment { get; set; }


        [Display(Name = "Продажа?")]
        public bool IsForSale { get; set;}

        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Region> Regions { get; set; }

    }
}