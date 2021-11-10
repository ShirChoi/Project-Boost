using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBoost.Models {
    public class User {
        [Key]
        public int ID { get; set; }//TODO поменять с int на Guid
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public char Type { get; set; } //  c - client, a - admin
        public new UserType GetType => Type == 'c' ? UserType.Client : UserType.Admin;
        public bool OpenFinantialHistory { get; set; }
        public bool Restricted { get; set; }

        //внешние штуки
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Complaint> Complaints { get; set; }
    }

    public enum UserType {
        Client = 0,
        Admin = 1,
    }
}
