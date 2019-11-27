using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegNumStore.Models
{
    public class LastModifiedViewModel
    {
        public string TagID { get; set; }

        //   [Required(ErrorMessage = "Пожалуйста, введите дату публикации статьи")]
        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}