using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esercitazione_1
{
    internal class DBManager
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Ticketing;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        internal List<Ticket> GetAll()
        {
            SqlConnection connection= new SqlConnection(connectionString);
            
            connection.Open();
            string query = "select * from Ticket order by Data";
            SqlCommand cmd = new SqlCommand(query, connection);

            SqlDataReader reader= cmd.ExecuteReader();
            List<Ticket> tickets = new List<Ticket>();
            while(reader.Read())
            {
                var id = reader.GetInt32(0); 
                var descr=reader.GetString(1);
                var data = reader.GetDateTime(2);
                var utente=reader.GetString(3);
                var stato=reader.GetString(4);

                Ticket t= new Ticket();
                t.Id = id;
                t.Utente = utente;
                t.Descrizione = descr;
                t.Stato = stato;
                t.Data = data;

                tickets.Add(t);
            }
            connection.Close();
            return tickets;
        }

        internal bool Add(Ticket t)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();
            string query = "insert Ticket values(@d,@data,@u,@s)";
            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@d", t.Descrizione);
            cmd.Parameters.AddWithValue("@data", t.Data);
            cmd.Parameters.AddWithValue("@u", t.Utente);
            cmd.Parameters.AddWithValue("@s", t.Stato);

            int righe = cmd.ExecuteNonQuery();
            if (righe >= 1)
                return true;
            else
                return false;

            connection.Close();
            
        }

        internal bool Delete(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();
            string query = "delete from Ticket where Id=@id";
            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@id", id);

            int righe = cmd.ExecuteNonQuery();
            if (righe >= 1)
                return true;
            else
                return false;

            connection.Close();
        }
    }
}
