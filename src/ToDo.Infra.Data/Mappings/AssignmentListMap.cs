using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Entities;

namespace ToDo.Infra.Data.Mappings;

public class AssignmentListMap : IEntityTypeConfiguration<AssignmentList>
{
    public void Configure(EntityTypeBuilder<AssignmentList> builder)
    {
        builder.ToTable("AssignmentList");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .UseMySqlIdentityColumn()
            .HasColumnType("BIGINT");

        builder.Property(a => a.Name)
            .IsRequired()
            .HasColumnType("VARCHAR(20)");
        
        builder.Property(a => a.Id)
            .IsRequired()
            .HasColumnType("BIGINT");

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasColumnType("BIGINT");
        builder.Property(a => a.ListId)
            .IsRequired()
            .HasColumnType("BIGINT");
    }
}