using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Entities;

namespace ToDo.Infra.Data.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Id)
            .UseMySqlIdentityColumn()
            .HasColumnType("BIGINT");

        builder.Property(u => u.Name)
            .IsRequired()
            .HasColumnType("VARCHAR(70)");

        builder.Property(u => u.Email)
            .IsRequired()
            .HasColumnType("VARCHAR(180)");
        
        builder.HasIndex(u => u.Email).IsUnique();
        
        builder.Property(u => u.Password)
            .IsRequired()
            .HasColumnType("VARCHAR(40)");
        
        builder.Property(u => u.AdminPrivileges)
            .HasDefaultValue(0)
            .HasColumnType("TINYINT");

    


    }
}