using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.CoreLayer.Entities;

namespace UserService.InfrastrucureLayer.DBcontext
{
    public class UserDBContext :DbContext
    {
        private readonly DbContextOptions<UserDBContext> _dbContextOptions;
        public UserDBContext(DbContextOptions<UserDBContext> dbContextOptions):base(dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserDBConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
