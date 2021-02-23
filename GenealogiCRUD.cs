using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Text;

namespace MyFamilyTree
{
    class GenealogiCRUD
    {
        public static string DatabaseName { get; set; } = "Genealogi";
        public static List<string> NamesOfTables = new List<string>();
        public static List<string> ColNames = new List<string>();
        public static List<int> Cursor = new List<int>();
        
        /// <summary>
        /// Skapar en databas med namnet Genealogi
        /// </summary>
        public static void CreateDatabase()
        {
            string cmdStr = "CREATE DATABASE " + DatabaseName + " ; ";
            string ConnectionString = String.Format(Program.ConnectionAddr, "");
            using (var cnn = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCmd = new SqlCommand(cmdStr, cnn);
                try
                {
                    cnn.Open();
                    sqlCmd.ExecuteNonQuery();

                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        /// <summary>
        /// Metoden kontrollerar om det redan finns en databas med angivet namn 
        /// </summary>
        /// <param name="databaseName">Namn på databasen</param>
        public static bool CheckDatabaseExists(string databaseName)
        {
            string sqlCreateDBQuery;
            bool result = false;

            try
            {
                var tmpConn = new SqlConnection(Program.ConnectionString);

                sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", databaseName);

                using (tmpConn)
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                    {
                        tmpConn.Open();

                        object resultObj = sqlCmd.ExecuteScalar();

                        int databaseID = 0;

                        if (resultObj != null)
                        {
                            int.TryParse(resultObj.ToString(), out databaseID);
                            Console.WriteLine($"Databas med namnet {databaseName} finns redan");
                        }
                        else Console.WriteLine($"Databas med namnet {databaseName} existerar inte. Databasen skapas.");

                        tmpConn.Close();

                        result = (databaseID > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Metoden kontrollerar om angivet tabellnamn redan existerar i databasen
        /// </summary>
        /// <param name="datatableName"></param>
        /// <returns></returns>
        public static bool CheckDataTableExists(string datatableName)
        {
            bool result = false;
            GetNamesOfTables();
            foreach (var item in NamesOfTables)
            {
                if (item == datatableName)
                {
                    result = true;
                    Console.WriteLine($" Tabell med namn {datatableName} finns redan");
                }
            }
            return result;
        }

        /// <summary>
        /// Skapar en tabell 
        /// </summary>
        /// <param name="tableName">Tabellnamn</param>
        public static void CreateDataTable(string tableName)
        {
            string sqlComStrtring = "ID int IDENTITY(1,1) PRIMARY KEY , Namn varchar(50), Efternamn varchar(50), Stad varchar(50), Ålder int, Mor int, Far int";
            try
            {
                using (var cnn = new SqlConnection(Program.ConnectionString))
                {
                    cnn.Open();
                    using (var command = new SqlCommand($"CREATE TABLE {tableName} ({sqlComStrtring});", cnn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /* Avancerad sätt att skapa tabeller ....
        public static void CreateDataTable(string tableName)
        {
            string columns = "";
            string loopStop;
            do
            {
                Console.Write("Lägg till kolumn: ");
                columns = columns + Console.ReadLine();
                Console.WriteLine("Välj datatyp (t.ex.: int, varchar(50), NOT NULL, PRIMARY KEY): ");
                columns = columns + " " + Console.ReadLine();
                Console.Write("Vill du lägga en till kolumn (y/n)?: ");
                loopStop = Console.ReadLine();
                if (loopStop == "y")
                {
                    columns = columns + ", ";
                }
            } while (loopStop == "y");
            try
            {
                using (var cnn = new SqlConnection(Program.ConnectionString))
                {
                    cnn.Open();
                    using (var command = new SqlCommand($"CREATE TABLE {tableName} ({columns});", cnn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        */

        /// <summary>
        /// Metoden lägger till kolumner i tabellen
        /// </summary>
        /// <param name="tableName">Tabellnamn</param>
        public static void AddColumn(string tableName)
        {
            Console.Write("Lägg till kolumnnamn: ");
            string columns = Console.ReadLine();
            Console.Write("Välj datatyp (int, varchar(50), NOT NULL): ");
            columns = columns + " " + Console.ReadLine();
            try
            {
                using (var cnn = new SqlConnection(Program.ConnectionString))
                {
                    cnn.Open();
                    using (var command = new SqlCommand($"ALTER TABLE {tableName} ADD {columns};", cnn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Metoden skapar en List<string> NamesOfTables med alla tabeller som existerar i databasen Genealogi
        /// </summary>
        public static void GetNamesOfTables()
        {
            NamesOfTables = new List<string>();
            using (SqlConnection con = new SqlConnection(Program.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT TABLE_NAME from information_schema.tables", con))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {

                        int i = 1;
                        while (dr.Read())
                        {
                            Console.Write(i + ". ");
                            Console.WriteLine(dr[0].ToString());
                            NamesOfTables.Add(dr[0].ToString());
                            i++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Skriver ut tabellen i konsolen på ett läsbart sätt
        /// </summary>
        /// <param name="dtname">Tabellnamn</param>
        public static void PrintTable(string dtname)
        {
            Cursor.Add(3);
            Cursor.Add(14);
            Cursor.Add(24);
            Cursor.Add(35);
            Cursor.Add(42);
            Cursor.Add(47);
            Cursor.Add(52);
            GetColumns(dtname);
            for (int i = 0; i < ColNames.Count; i++)
            {
                Console.Write($"{ColNames[i]}");
                Console.SetCursorPosition(Cursor[i], Console.CursorTop);
                Console.Write("| ");
            }
            Console.WriteLine();
            Console.WriteLine("======================================================");
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(Program.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM {dtname}", con);
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        for (int i = 0; i < ColNames.Count; i++)
                        {
                            Console.Write($"{row[ColNames[i]]} ");
                            Console.SetCursorPosition(Cursor[i], Console.CursorTop);
                            Console.Write("| ");
                        }
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Skapar en List<string> ColNames med namn på alla kolumner i tabellen
        /// </summary>
        /// <param name="tablename">Tabellnamn</param>
        public static void GetColumns(string tablename)
        {
            ColNames = new List<string>();

            using (SqlConnection con = new SqlConnection(Program.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand($"SELECT column_name from information_schema.columns where TABLE_NAME = '{tablename}'", con))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {

                        int i = 1;
                        while (dr.Read())
                        {
                            //Console.Write(i + ". ");
                            //Console.WriteLine(dr[0].ToString());
                            ColNames.Add(dr[0].ToString());
                            i++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Raderar en person (all information) från tabellen
        /// </summary>
        /// <param name="ID">ID nummer på personen som ska raderas</param>
        public static void DeleteRow (int ID)
        {
            try
            {
                using (var cnn = new SqlConnection(Program.ConnectionString))
                {
                    cnn.Open();
                    using (var command = new SqlCommand($"DELETE FROM FamilyMembers WHERE ID ={ID};", cnn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Raderar en tabell 
        /// </summary>
        /// <param name="tablename">Namn på tabellen som ska raaderas</param>
        public static void DeleteTable(string tablename)
        {
            try
            {
                using (var cnn = new SqlConnection(Program.ConnectionString))
                {
                    cnn.Open();
                    using (var command = new SqlCommand($"DROP TABLE {tablename};", cnn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Metoden redigerar alla uppgifter om en person i tabellen
        /// </summary>
        /// <param name="ID">ID nummer på personen som ska redigeras</param>
        public static void UpdatePerson(int ID)
        {
            Console.Write("Namn: ");
            string namn = Console.ReadLine();
            Console.Write("Efternamn: ");
            string efternamn = Console.ReadLine();
            Console.Write("Stad: ");
            string stad = Console.ReadLine();
            Console.Write("Ålder: ");
            string ålder = Console.ReadLine();
            Console.Write("Mor: ");
            string Mor = Console.ReadLine();
            Console.Write("Far: ");
            string far = Console.ReadLine();
            try
            {
                using (var cnn = new SqlConnection(Program.ConnectionString))
                {
                    cnn.Open();
                    using (var command = new SqlCommand($"UPDATE FamilyMembers SET Namn = '{namn}', Efternamn ='{efternamn}', Stad = '{stad}', Ålder = '{ålder}', Mor = '{Mor}', Far = '{far}' WHERE ID = {ID}", cnn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Redigerar bara uppgifter om föräldrar
        /// </summary>
        /// <param name="ID">ID nummer på personen som ska ändra info om förälsrar</param>
        /// <param name="mor">ID nummer på mor</param>
        /// <param name="far">ID nummer på far</param>
        public static void AddParents(int ID, int mor, int far)
        {
            try
            {
                using (var cnn = new SqlConnection(Program.ConnectionString))
                {
                    cnn.Open();
                    using (var command = new SqlCommand($"UPDATE FamilyMembers SET Mor = '{mor}', Far = '{far}' WHERE ID = {ID}", cnn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Söker en person på Namn
        /// </summary>
        /// <param name="namn">Namn som ska sökas</param>
        public static void FindPersonByName (string namn)
        {
            var dt = new DataTable();
            try
            {
                using (var cnn = new SqlConnection(Program.ConnectionString))
                {
                    cnn.Open();
                    using (var command = new SqlCommand($"SELECT * FROM FamilyMembers WHERE Namn = '{namn}'", cnn))
                    {
                        using (SqlDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                for (int i = 0; i < ColNames.Count; i++)
                                {
                                    Console.Write(dr[i].ToString() + " | ");
                                }
                                Console.WriteLine();
                            }
                            
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Skriver ut information om föräldrar
        /// </summary>
        /// <param name="ID">ID nummret på personen (barnet)</param>
        public static void ShowParents(int ID)
        {
            var dt = new DataTable();
            try
            {
                using (var cnn = new SqlConnection(Program.ConnectionString))
                {
                    cnn.Open();
                    using (var commandMor = new SqlCommand($"SELECT * FROM FamilyMembers WHERE ID = (SELECT Mor FROM FamilyMembers WHERE ID = {ID});", cnn))
                    {
                        var commandFar = new SqlCommand($"SELECT * FROM FamilyMembers WHERE ID = (SELECT Far FROM FamilyMembers WHERE ID = {ID});", cnn);
                        using (SqlDataReader dr = commandMor.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                for (int i = 0; i < ColNames.Count; i++)
                                {
                                    Console.Write(dr[i].ToString() + " | ");
                                }
                                Console.WriteLine();
                            }

                        }
                        using (SqlDataReader dr = commandFar.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                for (int i = 0; i < ColNames.Count; i++)
                                {
                                    Console.Write(dr[i].ToString() + " | ");
                                }
                                Console.WriteLine();
                            }

                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Metoden skriver ut far - och morföräldrar 
        /// </summary>
        /// <param name="ID">ID nummer på barnbarnet</param>
        public static void ShowGranParents(int ID)
        {
            var dt = new DataTable();
            try
            {
                using (var cnn = new SqlConnection(Program.ConnectionString))
                {
                    cnn.Open();
                    using (var commandMormor = new SqlCommand($"SELECT * FROM FamilyMembers	WHERE ID = (SELECT Mor FROM FamilyMembers WHERE ID = (SELECT Mor FROM FamilyMembers WHERE ID = {ID}));", cnn))
                    {
                        var commandFarMor = new SqlCommand($"SELECT * FROM FamilyMembers WHERE ID = (SELECT Far FROM FamilyMembers WHERE ID = (SELECT Mor FROM FamilyMembers WHERE ID = {ID}));", cnn);
                        var commandMorFar = new SqlCommand($"SELECT * FROM FamilyMembers WHERE ID = (SELECT Mor FROM FamilyMembers WHERE ID = (SELECT Far FROM FamilyMembers WHERE ID = {ID}));", cnn);
                        var commandFarFar = new SqlCommand($"SELECT * FROM FamilyMembers WHERE ID = (SELECT Far FROM FamilyMembers WHERE ID = (SELECT Far FROM FamilyMembers WHERE ID = {ID}));", cnn);
                        using (SqlDataReader dr = commandMormor.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                for (int i = 0; i < ColNames.Count; i++)
                                {
                                    Console.Write(dr[i].ToString() + " | ");
                                }
                                Console.WriteLine();
                            }

                        }
                        using (SqlDataReader dr = commandFarMor.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                for (int i = 0; i < ColNames.Count; i++)
                                {
                                    Console.Write(dr[i].ToString() + " | ");
                                }
                                Console.WriteLine();
                            }

                        }
                        using (SqlDataReader dr = commandMorFar.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                for (int i = 0; i < ColNames.Count; i++)
                                {
                                    Console.Write(dr[i].ToString() + " | ");
                                }
                                Console.WriteLine();
                            }

                        }
                        using (SqlDataReader dr = commandFarFar.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                for (int i = 0; i < ColNames.Count; i++)
                                {
                                    Console.Write(dr[i].ToString() + " | ");
                                }
                                Console.WriteLine();
                            }

                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Metoden skriver ut alla barn vars föräldrar är syskon
        /// </summary>
        /// <param name="ID">ID nummer på barnet</param>
        public static void ShowCousines(int ID)
        {
            var dt = new DataTable();
            try
            {
                using (var cnn = new SqlConnection(Program.ConnectionString))
                {
                    cnn.Open();
                    using (var commandCousine = new SqlCommand($"SELECT* FROM FamilyMembers	WHERE Far in (Select ID FROM FamilyMembers Where Far = (Select Far from FamilyMembers Where ID = (SELECT ID FROM FamilyMembers Where ID = (SELECT Far FROM FamilyMembers Where ID = {ID}))));", cnn))
                    {
                        using (SqlDataReader dr = commandCousine.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                for (int i = 0; i < ColNames.Count; i++)
                                {
                                    Console.Write(dr[i].ToString() + " | ");
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                }





            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
        /// <summary>
        /// Metoden skriver ut alla barn av en förälder
        /// </summary>
        /// <param name="ID">ID nummer på föräldern</param>
        public static void ShowChildren(int ID)
        {
            var dt = new DataTable();
            try
            {
                using (var cnn = new SqlConnection(Program.ConnectionString))
                {
                    cnn.Open();
                    using (var commandMor = new SqlCommand($"SELECT * FROM FamilyMembers WHERE Far = {ID} OR Mor = {ID};", cnn))
                    {
                        using (SqlDataReader dr = commandMor.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                for (int i = 0; i < ColNames.Count; i++)
                                {
                                    Console.Write(dr[i].ToString() + " | ");
                                }
                                Console.WriteLine();
                            }

                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Sorterar tabellen efter namnet
        /// </summary>
        /// <param name="dtname">Tabellnamn</param>
        /// <param name="orderByCol">Ange kolumnnamn som ska sorteras efter</param>>
        public static void OrderBy(string dtname, string orderByCol)
        {
            try
            {
                using (var cnn = new SqlConnection(Program.ConnectionString))
                {
                    cnn.Open();
                    using (var command = new SqlCommand($"SELECT * FROM {dtname} ORDER BY {orderByCol};", cnn))
                    {
                        using (SqlDataReader dr = command.ExecuteReader())
                        {
                            GetColumns(dtname);
                            while (dr.Read())
                            {
                                for (int i = 0; i < ColNames.Count; i++)
                                {
                                    Console.Write(dr[i].ToString() + " | ");
                                }
                                Console.WriteLine();
                            }

                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
    

}


