using System.ComponentModel.DataAnnotations;

namespace Esempio_EF._1.Entities
{
    public class Azienda
    {
        public int AziendaId { get; set; }
        [MaxLength(50)]
        public string Nome { get; set; }
        public int AnnoFondazione { get; set; }

        public List<Impiegato> Impiegati { get; set; } = new List<Impiegato>();      
    }
}