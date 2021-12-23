using Esempio_EF._2.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esempio_EF._2.EF
{
    internal class ImpiegatoConfiguration : IEntityTypeConfiguration<Impiegato>
    {
        public void Configure(EntityTypeBuilder<Impiegato> builder)
        {
            builder.ToTable("Impiegato");
            builder.HasKey(i => i.ImpiegatoId);
            builder.Property(i => i.Nome).IsRequired().HasMaxLength(50);
            builder.Property(i => i.Cognome).IsRequired().HasMaxLength(50);

            //relazioni
            builder.HasOne(i=>i.Azienda).WithMany(a=>a.Impiegati).HasForeignKey(i => i.AziendaId);

        }
    }
}