using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Domain;

namespace RegnumStore.Models
{
    public class LoginModel
    {
        
        [Display(Name = "Логин")]
        [DataType(DataType.Text)]
        [StringLength(Constants.LOGIN_MAX_LENGTH, MinimumLength = Constants.LOGIN_MIN_LENGTH, ErrorMessage = "Длина логина должна быть в диапазоне 3-20 символов")]
        [Required(ErrorMessage = "Необходимо корректно заполнить поле \"Логин\"")]
        public string UserName { get; set; }

        
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(Constants.PASSWORD_MAX_LENGTH, MinimumLength = Constants.PASSWORD_MIN_LENGTH, ErrorMessage = "Длина пароля должна быть в диапазоне 6-20 символов")]
        [Required(ErrorMessage = "Необходимо корректно заполнить поле \"Пароль\"")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }


        [Display(Name = "Докажите, что вы не робот")]
        public string Captcha { get; set; }
    }

    public class RegisterModel
    {
        [HiddenInput(DisplayValue = false)]
        public int UserID { get; set; }

        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Необходимо корректно заполнить поле Логин")]
        [StringLength(Constants.LOGIN_MAX_LENGTH, MinimumLength = Constants.LOGIN_MIN_LENGTH, ErrorMessage = "Длина логина должна быть в диапазоне 3-20 символов")]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [StringLength(Constants.PASSWORD_MAX_LENGTH, MinimumLength = Constants.PASSWORD_MIN_LENGTH, ErrorMessage = "Длина пароля должна быть в диапазоне 6-20 символов")]
        [Required(ErrorMessage = "Необходимо корректно заполнить поле Пароль")]
        [DataType(DataType.Password)]
        [MinLength(7, ErrorMessage = "Длина пароля должна быть более 6 символов")]
        public string Password { get; set; }

        // [Required(ErrorMessage = "Длина пароля должна быть более 6 символов")]
        //[MinLength(7, ErrorMessage = "Длина пароля должна быть более 6 символов")]
        [StringLength(Constants.PASSWORD_MAX_LENGTH, MinimumLength = Constants.PASSWORD_MIN_LENGTH, ErrorMessage = "Длина пароля должна быть в диапазоне 6-20 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Повтор пароля")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароль подтвержден неверно")]
        public string ConfirmPassword { get; set; }

        //  [Required(ErrorMessage = "Старый пароль указан неверно!")]
        [DataType(DataType.Password)]
        [StringLength(Constants.PASSWORD_MAX_LENGTH, MinimumLength = Constants.PASSWORD_MIN_LENGTH, ErrorMessage = "Длина пароля должна быть в диапазоне 6-20 символов")]
        [Display(Name = "Существующий пароль")]
        public string OldPassword { get; set; }
        
        [Required(ErrorMessage = "Необходимо заполнить поле Email")]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9.-]{1,20}@[a-zA-Z0-9.-]{1,20}\.[A-Za-z]{2,4}", ErrorMessage = "Неверный формат Email")]
        public string Email { get; set; }


        [Display(Name = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Необходимо заполнить поле  \"Телефон\"")]
        public string Phone { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Необходимо заполнить поле Имя")]
        [StringLength(Constants.LOGIN_MAX_LENGTH, MinimumLength = Constants.LOGIN_MIN_LENGTH, ErrorMessage = "Длина имени должна быть в диапазоне 3-20 символов")]
        public string UserName { get; set; }

        public DateTime Created { get; set; }

        public string PasswordSalt { get; set; }
        [DefaultValue(false)]
        public bool IsActivated { get; set; }
    }

    public class EditUserModel
    {
        [HiddenInput(DisplayValue = false)]
        public int UserID { get; set; }

        [Display(Name = "Логин")]
        [StringLength(Constants.LOGIN_MAX_LENGTH, MinimumLength = Constants.LOGIN_MIN_LENGTH, ErrorMessage = "Длина логина должна быть в диапазоне 3-20 символов")]
        [Required(ErrorMessage = "Необходимо корректно заполнить поле Логин")]
        public string Login { get; set; }
        
        [Required(ErrorMessage = "Необходимо заполнить поле Email")]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9.-]{1,20}@[a-zA-Z0-9.-]{1,20}\.[A-Za-z]{2,4}", ErrorMessage = "Неверный формат Email")]
        public string Email { get; set; }

        [Display(Name = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Необходимо заполнить поле Email")]
        public string Phone { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Необходимо заполнить поле Имя")]
        public string UserName { get; set; }
    }

}
