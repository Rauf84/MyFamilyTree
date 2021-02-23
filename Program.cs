using System;
using System.Dynamic;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace MyFamilyTree
{
    class Program
    {
        public static string ConnectionAddr { get; set; } = @"Data Source=.\SQLExpress;Integrated Security=true;";
        public static string ConnectionString { get; set; } = string.Format(@"Data Source =.\SQLExpress;Integrated Security = true;database={0}",GenealogiCRUD.DatabaseName);

        static void Main(string[] args)
        {

            if (GenealogiCRUD.CheckDatabaseExists(GenealogiCRUD.DatabaseName) == false)
            {
                GenealogiCRUD.CreateDatabase();
            }
            Console.WriteLine("* * * Välkommen till mitt familjträ * * * ");
            Console.WriteLine("Information om mina familjemedlemmar finns i databasen Genealogi.");
            Console.WriteLine("Databasen är skapat och programmet kontrollerar existensen varje gång vid start.");

            Console.WriteLine();
            Console.WriteLine("****************************************************************************************");
            Console.WriteLine("Programmet kontrollerar existens av FamilyMembers tabellen och skapar om den inte finns.");
            if (GenealogiCRUD.CheckDataTableExists("FamilyMembers") == false)
            {
                GenealogiCRUD.CreateDataTable("FamilyMembers");
                Console.WriteLine("Programmet har skapar en tabell som heter FamilyMembers");
            }
            Console.WriteLine("****************************************************************************************");
            // Skapar 14 familjemedlemmar i tabellen
            /*
            FamilyMember.CreatePerson("Rauf","Karimli","Baku",36,0,0);
            FamilyMember.CreatePerson("Nigar", "Karimli", "Baku", 34, 0, 0);
            FamilyMember.CreatePerson("Fuad", "Kairmli", "Baku", 37, 0, 0);
            FamilyMember.CreatePerson("Aida", "Kairmli", "Tabriz", 36, 0, 0);
            FamilyMember.CreatePerson("Tusi", "Karimli", "Baku", 38, 0, 0);
            FamilyMember.CreatePerson("Sakina", "Kairmli", "Baku", 40, 0, 0);
            FamilyMember.CreatePerson("Sevda", "Karimli", "Baku", 59, 0, 0);
            FamilyMember.CreatePerson("Firdovsi", "Karimli", "Baku", 59, 0, 0);
            FamilyMember.CreatePerson("Sibel", "Karimli", "Göteborg", 12, 0, 0);
            FamilyMember.CreatePerson("Ayaz", "Karimli", "Göteborg", 2, 0, 0);
            FamilyMember.CreatePerson("Mikael", "karimli", "Göteborg", 8, 0, 0);
            FamilyMember.CreatePerson("Suri", "Karimli", "Göteborg", 10, 0, 0);
            FamilyMember.CreatePerson("Asena", "Karimli", "Göteborg", 10, 0, 0);
            FamilyMember.CreatePerson("Almila", "Karimli", "Göteborg", 8, 0, 0);
            */
            //FamilyMember.CreatePerson("Shahira", "Farajova", "Baku", 58, 0, 0);



            /* lägger till föräldrar 
            GenealogiCRUD.AddParents(3,7, 8);
            GenealogiCRUD.AddParents(5, 7, 8);
            GenealogiCRUD.AddParents(9, 2, 1);
            GenealogiCRUD.AddParents(10, 2, 1);
            GenealogiCRUD.AddParents(11, 6, 5);
            GenealogiCRUD.AddParents(12, 6, 5);
            GenealogiCRUD.AddParents(13, 4, 3);
            GenealogiCRUD.AddParents(14, 4, 3);
            */


            GenealogiCRUD.PrintTable("FamilyMembers");

            Console.WriteLine();

            //GenealogiCRUD.ShowGranParents(10);

            Console.WriteLine();
            
            //Sorteringar
            /*
            GenealogiCRUD.OrderBy("FamilyMembers", "Namn");
            GenealogiCRUD.OrderBy("FamilyMembers", "Efternamn");
            GenealogiCRUD.OrderBy("FamilyMembers", "Stad");
            GenealogiCRUD.OrderBy("FamilyMembers", "Ålder");
            GenealogiCRUD.OrderBy("FamilyMembers", "Mor");
            GenealogiCRUD.OrderBy("FamilyMembers", "Far");
            */

            //GenealogiCRUD.ShowCousines(14);

            //GenealogiCRUD.ShowChildren(8);



            //FamilyMember.CreatePerson("Rauf", "Karimli", "Baku", 36, 0, 0); // existenskontroll

            //GenealogiCRUD.UpdatePerson(1); //Updaterar information om en person med ID nummer

            //GenealogiCRUD.FindPersonByName ("David"); // söker på namn och skriver ut

            //GenealogiCRUD.DeletePerson(16); //Raderar person med ID nummer

            //GenealogiCRUD.GetParents(1, 7, 8); // Tillsätter förälsrar till ID 1, med 8 och 9.

        }

    }     
}
