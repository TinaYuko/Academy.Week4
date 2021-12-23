using Esempio_EF._1.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esempio_EF._1.EF
{
    public class Context: DbContext
    {
        public DbSet<Impiegato> Impiegato { get; set; }
        public DbSet<Azienda> Azienda { get; set; }
        public Context(): base() 
        {

        }

        public Context(DbContextOptions<Context> options): base(options)    
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionStringSql = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AziendaEF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                optionsBuilder.UseSqlServer(connectionStringSql);   
            }
        }
    }
}
