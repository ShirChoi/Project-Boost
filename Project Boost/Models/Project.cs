using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBoost.Models {
    public class Project {
        [Key]
        public Guid ID { get; set; } 
        [ForeignKey("User")]
        public Guid UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Demo { get; set; } //TODO поменять со string на File
        public decimal RequiredAmount { get; set; }
        public decimal ReceivedAmount { get; set; }
        public DateTime DeadLine { get; set; }
        public bool Blocked { get; set; }
        
        //Внешние штуки
        public virtual User User { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Complaint> Complaints { get; set; }
    }
}
