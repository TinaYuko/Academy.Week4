using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ado.Disconnected
{
    public static class AdoNetDisconnectedDemo
    {
        static string connectionStringSQL = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RicetteZio;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static void FillDataSet()
        {
            DataSet cocktailsZioDS = new DataSet();
            using SqlConnection conn = new SqlConnection(connectionStringSQL);
            try
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connesso al DB");
                else Console.WriteLine("Non connesso al DB");


                InizializzaDataSetEAdapter(cocktailsZioDS, conn);
                conn.Close();
                Console.WriteLine("Connessione chiusa");

                //da qui in poi lavoro in modalità disconnessa

                foreach (DataTable tabella in cocktailsZioDS.Tables)
                {
                    Console.WriteLine($"{tabella.TableName} - {tabella.Rows.Count}");
                }

                Console.WriteLine("\nCome è fatta la tabella Ingrediente nel dataset");
                foreach (DataColumn colonna in cocktailsZioDS.Tables["Ingrediente"].Columns)
                {
                    Console.WriteLine($"{colonna.ColumnName} - {colonna.DataType}");
                }
                Console.WriteLine("\nDi seguito le constraint");
                foreach (Constraint vincolo in cocktailsZioDS.Tables["Ingrediente"].Constraints)
                {
                    Console.WriteLine($"{vincolo.ConstraintName} - {vincolo.ExtendedProperties}");
                }
                Console.WriteLine("\n-----Ingredienti-----");
                foreach (DataRow item in cocktailsZioDS.Tables["Ingrediente"].Rows)
                {
                    Console.WriteLine($"{item["IdIngrediente"]} - {item["Nome"]}");
                }
            }
            catch (SqlException ex)
            { Console.WriteLine(ex.Message); }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
            finally
            {
                conn.Close();
            }
        }

        private static SqlDataAdapter InizializzaDataSetEAdapter(DataSet cocktailsZioDS, SqlConnection conn)
        {
            SqlDataAdapter zioAdapter = new SqlDataAdapter();

            //Fill
            zioAdapter.SelectCommand = new SqlCommand("Select * from Ingrediente", conn);

            //Insert
            //zioAdapter.InsertCommand = new SqlCommand("Insert into Ingrediente values (@nome, @udm)", conn);
            //zioAdapter.InsertCommand.Parameters.AddWithValue("@nome", "Nome");
            //zioAdapter.InsertCommand.Parameters.AddWithValue("@udm", "UnitàDiMisura");
            zioAdapter.InsertCommand = GeneraInsertCommand(conn);

            //Update
            zioAdapter.UpdateCommand = GeneraUpdateCommand(conn);

            //Delete
            zioAdapter.DeleteCommand = GeneraDeleteCommand(conn);

            //Evita di dover definire a mano le primary key nelle tabelle
            zioAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            //E poi Fillo tutto
            zioAdapter.Fill(cocktailsZioDS, "Ingrediente");

            return zioAdapter;
        }

        private static SqlCommand GeneraDeleteCommand(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Delete from Ingrediente where IdIngrediente=@id";

            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "IdIngrediente"));
            return cmd;
        }

        private static SqlCommand GeneraUpdateCommand(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Update Ingrediente set Nome=@nome, UnitàDiMisura=@udm where IdIngrediente=@id";

            cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.NVarChar, 50, "Nome"));
            cmd.Parameters.Add(new SqlParameter("@udm", SqlDbType.NVarChar, 10, "UnitàDiMisura"));
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "IdIngrediente"));
            return cmd;
        }

        private static SqlCommand GeneraInsertCommand(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Insert into Ingrediente values (@nome, @udm)";

            cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.NVarChar, 50, "Nome"));
            cmd.Parameters.Add(new SqlParameter("@udm", SqlDbType.NVarChar, 10, "UnitàDiMisura"));
            return cmd;
        }
        public static void InsertRowDemo()
        {
            DataSet cocktailsZioDS = new DataSet();
            using SqlConnection conn = new SqlConnection(connectionStringSQL);
            try
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connesso al DB");
                else Console.WriteLine("Non connesso al DB");


                var zioAdapter = InizializzaDataSetEAdapter(cocktailsZioDS, conn);
                conn.Close();
                Console.WriteLine("Connessione chiusa");

                //da qui in poi lavoro in modalità disconnessa

                DataRow nuovaRiga = cocktailsZioDS.Tables["Ingrediente"].NewRow();
                nuovaRiga["Nome"] = "Cannella";
                nuovaRiga["UnitàDiMisura"] = "g";
                cocktailsZioDS.Tables["Ingrediente"].Rows.Add(nuovaRiga);
                //Ho aggiunto una nuova riga nella tabella Ingrediente del DatSet

                zioAdapter.Update(cocktailsZioDS, "Ingrediente");
                Console.WriteLine("DB aggiornato!");
                //riconciliazione e quindi vero salvataggio del dato sul DB
            }
            catch (SqlException ex)
            { Console.WriteLine(ex.Message); }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
            finally
            {
                conn.Close();
            }
        }

        public static void UpdateRowDemo()
        {
            DataSet cocktailsZioDS = new DataSet();
            using SqlConnection conn = new SqlConnection(connectionStringSQL);
            try
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connesso al DB");
                else Console.WriteLine("Non connesso al DB");


                var zioAdapter = InizializzaDataSetEAdapter(cocktailsZioDS, conn);
                conn.Close();
                Console.WriteLine("Connessione chiusa");

                //da qui in poi lavoro in modalità disconnessa

                DataRow rigaDaAggiornare = cocktailsZioDS.Tables["Ingrediente"].Rows.Find(4);
                if (rigaDaAggiornare != null)
                {
                    rigaDaAggiornare["UnitàDiMisura"] = "g";
                }

                //la connessione è chiusa, quindi devo fare la riconciliazione sul DB
                zioAdapter.Update(cocktailsZioDS, "Ingrediente");
                Console.WriteLine("DB aggiornato!");
            }
            catch (SqlException ex)
            { Console.WriteLine(ex.Message); }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
            finally
            {
                conn.Close();
            }
        }

        public static void DeleteRowDemo()
        {
            DataSet cocktailsZioDS = new DataSet();
            using SqlConnection conn = new SqlConnection(connectionStringSQL);
            try
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connesso al DB");
                else Console.WriteLine("Non connesso al DB");


                var zioAdapter = InizializzaDataSetEAdapter(cocktailsZioDS, conn);
                conn.Close();
                Console.WriteLine("Connessione chiusa");

                //da qui in poi lavoro in modalità disconnessa
                DataRow rigaDaEliminare = cocktailsZioDS.Tables["Ingrediente"].Rows.Find(26);
                if (rigaDaEliminare != null)
                {
                    rigaDaEliminare.Delete();
                }

                //la connessione è chiusa, quindi devo fare la riconciliazione sul DB
                zioAdapter.Update(cocktailsZioDS, "Ingrediente");
                Console.WriteLine("DB aggiornato!");
            }
            catch (SqlException ex)
            { Console.WriteLine(ex.Message); }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
            finally
            {
                conn.Close();
            }
        }

        public static void MultiTabelleDemo()
        {
            DataSet cocktailsZioDS = new DataSet();
            using SqlConnection conn = new SqlConnection(connectionStringSQL);
            try
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connesso al DB");
                else Console.WriteLine("Non connesso al DB");

                //Faccio i doppi fill 
                //Aggiungo libro al dataset
                SqlDataAdapter libroAdapter = new SqlDataAdapter();
                libroAdapter.SelectCommand = new SqlCommand("Select * from Libro", conn);
                libroAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                libroAdapter.Fill(cocktailsZioDS, "Libro");

                //aggiungo cocktail al dataset
                SqlDataAdapter cocktailsAdapter = new SqlDataAdapter();
                cocktailsAdapter.SelectCommand = new SqlCommand("Select * from Cocktail", conn);
                cocktailsAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                cocktailsAdapter.Fill(cocktailsZioDS, "Cocktail");

                conn.Close();
                Console.WriteLine("Connessione chiusa");

                //da qui in poi lavoro in modalità disconnessa
                foreach (DataTable item in cocktailsZioDS.Tables)
                {
                    Console.WriteLine($"{item.TableName} - {item.Rows.Count}");
                }
            }
            catch (SqlException ex)
            { Console.WriteLine(ex.Message); }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
            finally
            {
                conn.Close();
            }
        }

        public static void CostraintDemo()
            {
            DataSet cocktailsZioDS = new DataSet();
            using SqlConnection conn = new SqlConnection(connectionStringSQL);
            try
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connesso al DB");
                else Console.WriteLine("Non connesso al DB");

                //Faccio i doppi fill 
                //Aggiungo libro al dataset
                SqlDataAdapter libroAdapter = new SqlDataAdapter();
                libroAdapter.SelectCommand = new SqlCommand("Select * from Libro", conn);
                libroAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                libroAdapter.Fill(cocktailsZioDS, "Libro");

                //aggiungo cocktail al dataset
                SqlDataAdapter cocktailsAdapter = new SqlDataAdapter();
                cocktailsAdapter.SelectCommand = new SqlCommand("Select * from Cocktail", conn);
                cocktailsAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                cocktailsAdapter.Fill(cocktailsZioDS, "Cocktail");

                //Unique
                DataTable tabellaLibro = cocktailsZioDS.Tables["Libro"];
                UniqueConstraint titoloUnique = new UniqueConstraint(tabellaLibro.Columns["Titolo"]);
                tabellaLibro.Constraints.Add(titoloUnique);

                //FK
                DataColumn colonnaPadre = cocktailsZioDS.Tables["Libro"].Columns["IdLibro"];
                DataColumn colonnaFiglio = cocktailsZioDS.Tables["Cocktail"].Columns["IdLibro"];
                ForeignKeyConstraint fk_Libro = new ForeignKeyConstraint("FK_Libro",colonnaPadre, colonnaFiglio);
                cocktailsZioDS.Tables["Cocktail"].Constraints.Add(fk_Libro);

                conn.Close();
                Console.WriteLine("Connessione chiusa");

                //da qui in poi lavoro in modalità disconnessa
                foreach (DataTable item in cocktailsZioDS.Tables)
                {
                    Console.WriteLine($"{item.TableName} - {item.Rows.Count}");
                    foreach (Constraint c in item.Constraints)
                    {
                        Console.WriteLine($"{c.ConstraintName}");
                        if (c is UniqueConstraint)
                        {
                            //Stampa proprietà vincolo
                            StampaProprietàDelVincoloUnique(c);
                        }
                        else if (c is ForeignKeyConstraint)
                        {
                            //stampa le proprietà della fk
                            StampaProprietàDelVincoloFK(c);
                        }
                        Console.WriteLine("");
                    }
                    Console.WriteLine("--------------------\n");

                }
                
            }
            catch (SqlException ex)
            { Console.WriteLine(ex.Message); }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
            finally
            {
                conn.Close();
            }
        }

        private static void StampaProprietàDelVincoloFK(Constraint c)
        {
            ForeignKeyConstraint vincoloFK = (ForeignKeyConstraint)c;
            DataColumn[] arrayDiColonne = vincoloFK.Columns;
            for (int i = 0; i < arrayDiColonne.Length; i++)
            {
                Console.WriteLine($"Nome colonna: {arrayDiColonne[i].ColumnName}");
            }
        }

        private static void StampaProprietàDelVincoloUnique(Constraint c)
        {
            UniqueConstraint vincoloUnique=(UniqueConstraint)c;
            DataColumn[] arrayDiColonne= vincoloUnique.Columns;
            for (int i = 0; i < arrayDiColonne.Length; i++)
            {
                Console.WriteLine($"Nome colonna: {arrayDiColonne[i].ColumnName}");
            }
        }
    }
}
