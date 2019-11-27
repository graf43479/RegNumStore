using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Comment
    {
        public int CommentID { get; set; }

        [Required(ErrorMessage = "Введите тему письма")]
        public string Tittle { get; set; }

        [Required(ErrorMessage = "Введите текст вопроса")]
        [MinLength(15, ErrorMessage = "Длинна текста слишком маленькая")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Введите текст ответа")]
        //[MinLength(15, ErrorMessage = "Длинна текста слишком маленькая")]
        public string AnswerText { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime AnswerDate { get; set; }
        public bool IsAccept { get; set; }

    }
}
