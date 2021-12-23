using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ado
{

    static class AdoNetDemo
    {
        static string connectionStringSQL = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RicetteZio;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static void ConnectionDemo()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            connessione.Open();

            if (connessione.State==System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Connessi al DB");
            }
            else
            {
                Console.WriteLine("NON connessi al DB");
            }
            connessione.Close();
        }  
        
        public static void DataReaderDemo()
        {
            using SqlConnection connessione= new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connessi al DB");
                }
                else
                {
                    Console.WriteLine("NON connessi al DB");
                }

                string query = "select * from Ingrediente";

                //Istanziare SqlCommand pt.1
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connessione;
                cmd.CommandType=System.Data.CommandType.Text;
                cmd.CommandText=query;

                //Istanziare SqlCommand pt.2
                SqlCommand cmd2 = new SqlCommand(query, connessione);

                //Istanziare SqlCommand pt.3
                SqlCommand cmd3 = connessione.CreateCommand();
                cmd3.CommandText=query;

                SqlDataReader reader = cmd.ExecuteReader();
                Console.WriteLine("---Ingredienti---");
                while (reader.Read())
                {
                    //var id = reader.GetInt32(0); //intero -> lettura tipizzata
                    //var nome=reader.GetString(1); //stringa
                    //var unità=reader.GetString(2);

                    var id = (int)reader["IdIngrediente"];
                    var nome = (string)reader["Nome"];
                    var unità = (string)reader["UnitàDiMisura"];

                    Console.WriteLine($"{id} - {nome}");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
            finally
            {
                connessione.Close();
                Console.WriteLine("\nConnessione chiusa");
            }
        }
        public static void InsertDemo()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connessi al DB");
                }
                else
                {
                    Console.WriteLine("NON connessi al DB");
                }
                //facciamo finta che ho chiesto all'utente il nuovo ingrediente
                string ingrediente = "Cocco";
                string unità = "ml";
                
                //string query = "insert into Ingrediente values ('Cocco', 'ml')";
                string query = $"insert into Ingrediente values ('{ingrediente}','{unità}')";

                SqlCommand cmd = connessione.CreateCommand();
                cmd.CommandText = query;

                int righe= cmd.ExecuteNonQuery();
                if (righe>=1)
                    Console.WriteLine($"{righe} row insert");
                else
                    Console.WriteLine("Ops, qualcosa è andato storto!");

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
            finally
            {
                connessione.Close();
                Console.WriteLine("\nConnessione chiusa");
            }
        }

        public static void InserConParametriDemo()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connessi al DB");
                }
                else
                {
                    Console.WriteLine("NON connessi al DB");
                }
                //facciamo finta che ho chiesto all'utente il nuovo ingrediente
                string ingrediente = "Peperoncino";
                string unità = "g";

                //string query = "insert into Ingrediente values ('Cocco', 'ml')";
                string query = "insert into Ingrediente values (@i, @u)";
                SqlCommand cmd=connessione.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@i", ingrediente);
                //cmd.Parameters.AddWithValue("@u", unità);
                //posso anche fare così
                SqlParameter udmParametro = new SqlParameter();
                udmParametro.ParameterName = "@u";
                udmParametro.Value = unità;
                udmParametro.DbType = System.Data.DbType.String;
                udmParametro.Size = 10;
                cmd.Parameters.Add(udmParametro);
                

                int righe = cmd.ExecuteNonQuery();
                if (righe >= 1)
                    Console.WriteLine($"{righe} row insert");
                else
                    Console.WriteLine("Ops, qualcosa è andato storto!");

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
            finally
            {
                connessione.Close();
                Console.WriteLine("\nConnessione chiusa");
            }
        }

        public static void DeleteConParametriDemo()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connessi al DB");
                }
                else
                {
                    Console.WriteLine("NON connessi al DB");
                }
                //facciamo finta che ho chiesto all'utente l'id
                int id = 24;

                string query = "delete from Ingrediente where IdIngrediente=@id";
                SqlCommand cmd = connessione.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", id);
                

                int righe = cmd.ExecuteNonQuery();
                if (righe >= 1)
                    Console.WriteLine($"{righe} row delete");
                else
                    Console.WriteLine("Ops, qualcosa è andato storto!");

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
            finally
            {
                connessione.Close();
                Console.WriteLine("\nConnessione chiusa");
            }
        }

        public static void StoredProcedureDemo()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connessi al DB");
                }
                else
                {
                    Console.WriteLine("NON connessi al DB");
                }

                //Chiedo all'utente i dati
                string nome = "Moscow Mule";
                string prep = "Mischia tutto";
                int tempo = 5;
                int persone = 4;
                string libro = "Cocktails chic";

                SqlCommand cmd = connessione.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "InserisciCocktail";

                //parametri
                cmd.Parameters.AddWithValue("@n", nome);
                cmd.Parameters.AddWithValue("@p", prep);
                cmd.Parameters.AddWithValue("@p", prep);
                cmd.Parameters.AddWithValue("@t", tempo);
                cmd.Parameters.AddWithValue("@persone", persone);
                cmd.Parameters.AddWithValue("@libro", libro);


                int righe = cmd.ExecuteNonQuery();
                if (righe >= 1)
                    Console.WriteLine($"{righe} row delete");
                else
                    Console.WriteLine("Ops, qualcosa è andato storto!");

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
            finally
            {
                connessione.Close();
                Console.WriteLine("\nConnessione chiusa");
            }
        }
        public static void RisultatiMlutipliDemo()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connessi al DB");
                }
                else
                {
                    Console.WriteLine("NON connessi al DB");
                }

                string queries = "Select * from Ingrediente; Select * from Cocktail";

                SqlCommand cmd= new SqlCommand(queries, connessione);
                SqlDataReader reader=cmd.ExecuteReader();

                int index = 0;
                while (reader.HasRows)
                {
                    Console.WriteLine($"Result set: {index +1}");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Nome"]}");
                    }
                    reader.NextResult();
                    index++;
                    Console.WriteLine("\n");
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
            finally
            {
                connessione.Close();
                Console.WriteLine("\nConnessione chiusa");
            }
        }

        public static void TransactionDemo()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            SqlTransaction tran = null;
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connessi al DB");
                }
                else
                {
                    Console.WriteLine("NON connessi al DB");
                }

                tran = connessione.BeginTransaction();

                string query = "Insert into Ingrediente values ('Corbezzolo','g')";

                SqlCommand cmd = new SqlCommand(query, connessione);
                cmd.Transaction= tran;

                int riga = cmd.ExecuteNonQuery();

                string query2 = "Insert into Libro values ('Cocktail classici')";

                SqlCommand cmd2 = new SqlCommand(query2, connessione);
                cmd2.Transaction = tran;

                riga = cmd2.ExecuteNonQuery();

                tran.Commit();

            }
            catch (SqlException ex)
            {
                tran.Rollback();
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                tran.Rollback();
                Console.WriteLine($"Errore: {ex.Message}");
            }
            finally
            {
                connessione.Close();
                Console.WriteLine("\nConnessione chiusa");
            }
        }
    }
}

