using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ProjectBoost.Models;

namespace ProjectBoost.Models.ViewModels.UserModels {
    public class UserEditModel {
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Логин не может быть пустым")]
        [Display(Name = "новый Логин")]
        public string Nickname { get; set; } = "trash2";
        [Required(ErrorMessage = "Пароль не может быть пустым")]
        [Display(Name = "новый Пароль")]
        public string Password { get; set; } = "trash2";

        [Display(Name = "Ограничен")]
        public bool Restricted { get; set; } = false;

        [Display(Name = "Публичная история платежей")]
        public bool OpenFinantialHistory { get; set; } = false;
    }
}
