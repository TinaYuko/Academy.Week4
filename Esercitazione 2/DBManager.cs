using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esercitazione_2
{
    internal class DBManager
    {
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Ticketing;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private static SqlDataAdapter InizializzaData(DataSet ticketDS, SqlConnection conn)
        {
            SqlDataAdapter ticketAdapter = new SqlDataAdapter();

            //Fill
            ticketAdapter.SelectCommand = new SqlCommand("Select * from Ticket order by Data", conn);

            //INSERT
            ticketAdapter.InsertCommand = GeneraInsertCommand(conn);

            //UPDATE
            ticketAdapter.UpdateCommand = GeneraUpdateCommand(conn);

            //DELETE
            ticketAdapter.DeleteCommand = GeneraDeleteCommand(conn);


            //evita di dover definire a mano le PK nelle tabelle
            ticketAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            ticketAdapter.Fill(ticketDS, "Ticket");
            return ticketAdapter;
        }

        private static SqlCommand GeneraDeleteCommand(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Delete from Ticket where Id=@id";

            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "Id"));

            return cmd;
        }

        private static SqlCommand GeneraUpdateCommand(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Update Ticket set Descrizione=@d, Data=@data, Utente=@u, Stato=@s where Id=@id";

            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "Id"));
            cmd.Parameters.Add(new SqlParameter("@d", SqlDbType.NVarChar, 50, "Descrizione"));
            cmd.Parameters.Add(new SqlParameter("@data", SqlDbType.Date, 0, "Data"));
            cmd.Parameters.Add(new SqlParameter("@u", SqlDbType.NVarChar, 50, "Utente"));
            cmd.Parameters.Add(new SqlParameter("@s", SqlDbType.NVarChar, 10, "Stato"));

            return cmd;
        }

        private static SqlCommand GeneraInsertCommand(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Insert into Ticket values (@d, @data, @u, @s)";

            cmd.Parameters.Add(new SqlParameter("@d", SqlDbType.NVarChar, 50, "Descrizione"));
            cmd.Parameters.Add(new SqlParameter("@data", SqlDbType.Date, 0, "Data"));
            cmd.Parameters.Add(new SqlParameter("@u", SqlDbType.NVarChar, 50, "Utente"));
            cmd.Parameters.Add(new SqlParameter("@s", SqlDbType.NVarChar, 10, "Stato"));

            return cmd;
        }

        public static List<Ticket> GetAll()
        {
            DataSet ticketDS = new DataSet();
            using SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al db");
                else
                    Console.WriteLine("NON connessi al db");

                InizializzaData(ticketDS, conn);

                conn.Close();
                Console.WriteLine("Connessione chiusa");

                //da qui lavoro in modalità disconnessa-> sono offline
                List<Ticket> tickets = new List<Ticket>();


                foreach (DataRow item in ticketDS.Tables["Ticket"].Rows)
                {
                    Ticket t = new Ticket();
                    t.Id = (int)item["Id"];
                    t.Utente = (string)item["Utente"];
                    t.Descrizione = (string)item["Descrizione"];
                    t.Stato = (string)item["Stato"];
                    t.Data = (DateTime)item["Data"];
                    tickets.Add(t);
                }

                return tickets;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool Add(Ticket t)
        {
            DataSet ticketDS = new DataSet();
            using SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al db");
                else
                    Console.WriteLine("NON connessi al db");

                var ticketAdapter = InizializzaData(ticketDS, conn);
                conn.Close();
                Console.WriteLine("Connessione chiusa");

                DataRow nuovaRiga = ticketDS.Tables["Ticket"].NewRow();
                nuovaRiga["Descrizione"] = t.Descrizione;
                nuovaRiga["Data"] = t.Data;
                nuovaRiga["Utente"] = t.Utente;
                nuovaRiga["Stato"] = t.Stato;

                ticketDS.Tables["Ticket"].Rows.Add(nuovaRiga);
                //aggiunto la mia nuova riga nel dataset

                //riconciliazione e quindi vero salvataggio del dato sul db
                ticketAdapter.Update(ticketDS, "Ticket");
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool Delete(int id)
        {
            DataSet ticketDS = new DataSet();
            using SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al db");
                else
                    Console.WriteLine("NON connessi al db");

                var ticketAdapter = InizializzaData(ticketDS, conn);
                conn.Close();
                Console.WriteLine("Connessione chiusa");

                DataRow rigaDaEliminare = ticketDS.Tables["Ticket"].Rows.Find(id);
                //by PK
                if (rigaDaEliminare != null)
                {
                    rigaDaEliminare.Delete();
                }

                //riconciliazione e quindi vero salvataggio del dato sul db
                ticketAdapter.Update(ticketDS, "Ticket");
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
