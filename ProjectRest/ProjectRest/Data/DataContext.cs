using Microsoft.EntityFrameworkCore;
using ProjectRest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRest.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Integrated Security=SSPI;Persist Security Info=False;User ID=WHOAMI\\wHOAMI;Initial Catalog=restapi;Data Source=WHOAMI\\SQLEXPRESS");
        }
    }
}
