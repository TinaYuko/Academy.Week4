using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esercitazione_2
{
    internal class Menu
    {
        private static DBManager dbManager = new DBManager();
        internal static void Start()
        {
            Console.WriteLine("Benvenut* al database TicketOnline");
            bool continua = true;
            while (continua)
            {
                Console.WriteLine("Si prega di premere: ");
                Console.WriteLine("[1] Per stampare la lista dei Ticket in ordine cronologico");
                Console.WriteLine("[2] Per aggingere nuovi Ticket ");
                Console.WriteLine("[3] Per cancellare Ticket ");
                Console.WriteLine("[0] Per uscire");


                int scelta;
                do
                {
                    Console.WriteLine("Si prega di scegliere");
                } while (!(int.TryParse(Console.ReadLine(), out scelta) && scelta >= 0 && scelta < 4));

                switch (scelta)
                {
                    case 1:
                        StampaTicket();
                        break;
                    case 2:
                        AggiungiTicket();
                        break;
                    case 3:
                        EliminaTicket();
                        break;
                    case 0:
                        Console.WriteLine("La ringraziamo per aver visionato il nostro portale. Arrivederci!");
                        continua = false;
                        break;
                }
            }
        }

        private static void EliminaTicket()
        {
            StampaTicket();
            int id;
            Console.Write("Inserire l'Id del ticket da eliminare: ");
            while (!(int.TryParse(Console.ReadLine(), out id) && id > 0))
            {
                Console.WriteLine("Valore errato. Riprova:");
            }

            bool esito=DBManager.Delete(id);
            if (esito)
                Console.WriteLine("Ticket eliminato correttamente!");
            else
                Console.WriteLine("Qualcosa è andato storto!");
        }

        private static void AggiungiTicket()
        {
            Ticket t= new Ticket();
            Console.Write("Inserisci descrizione ticket: ");
            string descrizione= Console.ReadLine();
            DateTime data;
            Console.Write("Inserisci data ticket: ");
            while (!(DateTime.TryParse(Console.ReadLine(), out data)));
            {
                Console.WriteLine("Data errata. Riprova!");
            }
            Console.Write("Inserisci utente ticket: ");
            string utente= Console.ReadLine();
            Console.Write("Inserisci stato ticket: ");
            string stato= Console.ReadLine();

            t.Descrizione = descrizione;
            t.Data = data;
            t.Utente = utente;
            t.Stato = stato;

            bool esito=DBManager.Add(t);
            if (esito)
                Console.WriteLine("Ticket aggiunto correttamente!");
            else
                Console.WriteLine("Qualcosa è andato storto!");
        }

        private static void StampaTicket()
        {
            Console.WriteLine("Tutti i tickets presenti sono:");
            List<Ticket> tickets = DBManager.GetAll();

            if (tickets == null)
                Console.WriteLine("Nessun ticket presente!");
            else
            {
                foreach (Ticket ticket in tickets)
                    Console.WriteLine(ticket);
            }
        }
    }
}
