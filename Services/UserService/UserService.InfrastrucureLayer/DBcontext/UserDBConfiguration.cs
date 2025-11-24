using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.CoreLayer.Entities;

namespace UserService.InfrastrucureLayer.DBcontext
{
    public class UserDBConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(p => p.EmailAddress).IsUnique();
            builder.HasKey(p => p.ID);
        }
    }
}
