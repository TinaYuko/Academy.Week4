using Esempio_EF._2.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esempio_EF._2.EF
{
    internal class AziendaConfiguration : IEntityTypeConfiguration<Azienda>
    {
        public void Configure(EntityTypeBuilder<Azienda> builder)
        {
            builder.ToTable("Azienda"); //nome tabella
            builder.HasKey(a => a.AziendaId); //PK
            //builder.Property("Nome").IsRequired();
            builder.Property(a=>a.Nome).IsRequired();
            
            //relazioni
            //relazione azienda - impiegato 1:n
            builder.HasMany(a=>a.Impiegati).WithOne(i=>i.Azienda).HasForeignKey(i=>i.AziendaId);
        }
    }
}