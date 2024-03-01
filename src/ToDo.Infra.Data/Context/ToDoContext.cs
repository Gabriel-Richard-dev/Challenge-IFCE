using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;
using ToDo.Infra.Data.Mappings;


namespace ToDo.Infra.Data.Context;

public class ToDoContext : DbContext, IUnityOfWork
{
    public ToDoContext() { }

    public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<AssignmentList> AssignmentLists { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {

        string connection = "server=localhost; port=3306;database=TODODB; uid=root;password=Lab@inf019";
        
        builder.UseMySql(connection, ServerVersion.AutoDetect(connection));
        builder.EnableSensitiveDataLogging(true);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserMap());
        builder.ApplyConfiguration(new AssignmentMap());
        builder.ApplyConfiguration(new AssignmentListMap());
    }

    public async Task<bool> Commit() => await SaveChangesAsync() > 0; 

}