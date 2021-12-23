using Esempio_EF._2.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esempio_EF._2.EF
{
    internal class Context: DbContext
    {
        public DbSet<Impiegato> Impiegati { get; set; }
        public DbSet<Azienda> Aziende { get; set; }
        public Context() : base()
        {

        }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionStringSql = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AziendaAPI;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                optionsBuilder.UseSqlServer(connectionStringSql);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Impiegato>(new ImpiegatoConfiguration());
            modelBuilder.ApplyConfiguration<Azienda>(new AziendaConfiguration());
        }
    }
}
