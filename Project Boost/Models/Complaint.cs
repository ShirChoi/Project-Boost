using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBoost.Models {
    public class Complaint {
        [Key] 
        public int ID { get; set; }//TODO поменять с int на Guid

        [ForeignKey("User")] 
        public int UserID { get; set; }
        
        [ForeignKey("Project")] 
        public int ProjectID { get; set; }
        
        public string Text { get; set; }

        //внешние штуки 
        public virtual User User { get; set; }
        public virtual Project Project { get; set; }
    }
}
