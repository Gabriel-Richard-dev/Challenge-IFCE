using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Entities;

namespace ToDo.Infra.Data.Mappings;

public class AssignmentMap : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Id)
            .UseMySqlIdentityColumn()
            .HasColumnType("BIGINT");


        builder.Property(a => a.Title)
            .IsRequired()
            .HasColumnType("VARCHAR(20)");
        
        builder.Property(a => a.Description)
            .HasColumnType("VARCHAR(300)");

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasColumnType("BIGINT");

        builder.Property(a => a.AssignmentListId)
            .IsRequired()
            .HasColumnType("BIGINT");

        builder.Property(a => a.Concluded)
            .HasColumnType("TINYINT")
            .HasDefaultValue(0);

        builder.Property(a => a.DateConcluded)
            .HasColumnType("DATE")
            .IsRequired(false)
            .HasDefaultValue(null);

        builder.Property(a => a.Deadline)
            .HasColumnType("DATE");



    }
}