using System;
using System.ComponentModel.DataAnnotations;

namespace DiabeticCalculator.Models.IdentityUs.CRUDUser
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Не правильный emeil")]
        public string Email { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароль введен не верно")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}