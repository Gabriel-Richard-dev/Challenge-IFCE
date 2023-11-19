using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;
using ToDo.Infra.Data.Mappings;

namespace ToDo.Infra.Data.Context;

public class ToDoContext : DbContext
{
    public ToDoContext() { }
    public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<AssignmentList> AssignmentLists { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        var connection = "server=localhost; port=3306;database=TODODB; uid=root;password=0612";
        builder.UseMySql(connection, ServerVersion.AutoDetect(connection));
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
    
}