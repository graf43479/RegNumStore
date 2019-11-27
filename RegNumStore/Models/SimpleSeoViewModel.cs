using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegNumStore.Models
{
    public class SimpleSeoViewModel
    {

        public string TagID { get; set; }


        public string Keywords { get; set; }

        [Required(ErrorMessage = "Заполните сниппет")]
        [MaxLength(1000, ErrorMessage = "Количество символов не должно превышать 1000 ")]
        public string Snippet { get; set; }

        [Required(ErrorMessage = "Заполните title")]
        [MaxLength(500, ErrorMessage = "Количество символов не должно превышать 500 ")]
        public string Tittle { get; set; }
      
        [Required(ErrorMessage = "Заполните раздел Robots для индексации")]
        [MaxLength(100, ErrorMessage = "Количество символов не должно превышать 100 ")]
        public string Robots { get; set; }

    }
}