using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace ProjectBoost.Models.ViewModels.ProjectModels {
    public class ProjectEditModel {
        public Guid ID { get; set; }
        [Required]
        [Display(Name = "описание")]
        public string Description { get; set; } = "no";
        [Required]
        [Display(Name = "демо")]
        public string Demo { get; set; } = "no"; //TODO поменять со string на File
        [Required]
        [Display(Name = "необходимая сумма")]
        public decimal RequiredAmount { get; set; } = 0;
        [Required]
        [Display(Name = "крайний срок")]
        public DateTime DeadLine { get; set; } = DateTime.MaxValue;
        [Required]
        [Display(Name = "заблокировать")]
        public bool Blocked { get; set; } = false;
    }
}
