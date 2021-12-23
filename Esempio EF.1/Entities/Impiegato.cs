using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esempio_EF._1.Entities
{
    public class Impiegato
    {
        public int ImpiegatoId { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public DateTime DataNascita { get; set; }
        public int AziendaId { get; set; }
        public Azienda Azienda { get; set; }
    }
}
