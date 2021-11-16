using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBoost.Models.ViewModels.UserModels {
    public class UserLoginModel {
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Логин не может быть пустым")]
        [Display(Name = "Логин")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Пароль не может быть пустым")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
