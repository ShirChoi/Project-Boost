using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ProjectBoost.Models {
    public class User {
        [Key]
        public Guid ID { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public char Type { get; set; } //  c - client, a - admin
        public new UserType GetType() => Type == 'c' ? UserType.Client : UserType.Admin;
        public void SetType(UserType newType) => Type = (newType == UserType.Client ? 'c' : 'a');
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
