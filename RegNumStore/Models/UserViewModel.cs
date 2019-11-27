﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace RegnumStore.Models
{
    public class UserViewModel
    {
        public int UserID { get; set; }

     //   public int RoleID { get; set; }

        [Required(ErrorMessage = "Введите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(7, ErrorMessage = "Длинна пароля должна быть более 6 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите email")]
        [RegularExpression(@"^[a-zA-Z0-9.-]{1,20}@[a-zA-Z0-9.-]{1,20}\.[A-Za-z]{2,4}", ErrorMessage = "Неверный формат Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите телефон")]
        [MinLength(7, ErrorMessage = "Длинна телефона должна быть более 6 символов")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Введите ваше имя")]
        [MinLength(3, ErrorMessage = "Длинна имени должна быть более 3 символов")]
        public string UserName { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }


        public string PasswordSalt { get; set; }

        [DefaultValue(false)]
        public bool IsActivated { get; set; }

        public string NewEmailKey { get; set; }

        public IEnumerable<Role> Roles { get; set; }
        public string RoleName { get; set; }
        public int SelectedRoleID { get; set; }
    }
}