using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProjectBoost.Models.ViewModels.UserModels {
    public class UserDetailsModel {
        public Guid ID { get; set; }
        public int RoleID { get; set; }
        [Display(Name = "Логин")]
        public string Nickname { get; set; }

        [Display(Name = "Публичная история платежей")]
        public bool OpenFinantialHistory { get; set; }

        [Display(Name = "Ограничен")]
        public bool Restricted { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
