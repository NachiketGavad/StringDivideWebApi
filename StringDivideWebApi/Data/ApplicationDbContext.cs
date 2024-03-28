// ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using StringDivideWebApp.Models;
using System.Collections.Generic;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<StringDivideAppUser> StringDivideAppUsers { get; set; }
}