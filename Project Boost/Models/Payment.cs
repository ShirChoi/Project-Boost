using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBoost.Models {
    public class Payment {
        [Key]
        public Guid ID { get; set; }
        [ForeignKey("User")]
        public Guid UserID { get; set; }
        [ForeignKey("Project")]
        public Guid ProjectID { get; set; }
        [Required]
        public decimal Amount { get; set; }//TODO сделать так, чтобы нельзя было давать отрицательное кол-во денег

        //внешние штуки 
        public virtual User User { get; set; }
        public virtual Project Project { get; set; }

    }
}
