using Microsoft.EntityFrameworkCore;
using ProjectBoost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBoost.Context {
    public class ProjectBoostContext : DbContext {
        public ProjectBoostContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminName = "admin";
            string adminPassword = "password";

            // добавляем роли
            Role adminRole = new Role { ID = 1, Name = adminRoleName };
            Role userRole = new Role { ID = 2, Name = userRoleName };
            User adminUser = new User { 
                ID = Guid.NewGuid(), 
                Nickname = adminName, 
                Password = adminPassword, 
                RoleID = (int)adminRole.ID,
                Restricted = false,
                OpenFinantialHistory = false,
            };
           
            
            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
