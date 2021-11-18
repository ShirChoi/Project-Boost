using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProjectBoost.Models.ViewModels.ProjectModels {
    public class ProjectCreateModel {
        [Required]
        [Display(Name = "Имя проекта")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Описание проекта")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Демо проекта")]
        public string Demo { get; set; } //TODO поменять со string на File
        [Required]
        [Display(Name = "Необходимая сумма")]
        public decimal RequiredAmount { get; set; }
        [Required]
        [Display(Name = "Срок окончания ")]
        public DateTime DeadLine { get; set; }
    }
}
