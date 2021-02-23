using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace MyFamilyTree
{
    class FamilyMember
    {
        public static string sqlStringColumnNames = "";
        public static string sqlStringValue = "";
        public static string sqlString = "";

        /// <summary>
        /// Skapar familjemedlemmar med angivna parametrar
        /// </summary>
        /// <param name="namn">Namn</param>
        /// <param name="efternamn">Efternamn</param>
        /// <param name="stad">Födelsestad</param>
        /// <param name="ålder">Ålder</param>
        /// <param name="mor">ID nummer på mor / 0 om informationsaknas</param>
        /// <param name="far">ID nummer på far / 0 om informationsaknas</param>
        public static void CreatePerson(string namn, string efternamn, string stad, int ålder, int mor, int far)
        {
            var dt = new DataTable();
            sqlString = $"SELECT * FROM FamilyMembers WHERE Namn = '{namn}'";
            try
            {
                using (var cnn = new SqlConnection(Program.ConnectionString))
                {
                    cnn.Open();
                    using (var command1 = new SqlCommand(sqlString, cnn))
                    {
                        
                        SqlDataAdapter da = new SqlDataAdapter(command1);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Console.WriteLine("Det finns redan en person med detta namn");
                        }
                        else
                        {
                            Console.WriteLine("Person med detta namn finns inte i tabellen och ska skapas");
                            sqlString = $"INSERT INTO FamilyMembers (Namn, Efternamn, Stad, Ålder, Mor, Far) VALUES ('{namn}', '{efternamn}', '{stad}', {ålder}, {mor}, {far})";
                            using (var command2 = new SqlCommand(sqlString, cnn))
                            {
                                command2.ExecuteNonQuery();
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
