using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esercitazione_2
{
    /*ID(int, PK, auto-incrementale)
    Descrizione(varchar(500))
    Data(datetime)
    Utente(varchar(100))
    Stato(varchar(10)) – New, OnGoing, Resolved
    */
    internal class Ticket
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        public DateTime Data{ get; set; }
        public string Utente { get; set; }
        public string Stato { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Descrizione}. Data: {Data.ToShortDateString()} - Utente {Utente} - Stato: {Stato}";
        }
    }

   
}
