using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace TestCommande
{
    internal class Program
    {
        static void Main(string[] args)
        {


            Dictionary<string, string> lesStatutsString = new Dictionary<string, string>{
                { "1", "2023-02-20" },
                { "2", "2002-12-01" }
            };


            Dictionary<int, DateTime> lesStatuts = new Dictionary<int, DateTime>();



            foreach (string key in lesStatutsString.Keys)
            {
                Console.WriteLine(key + " : " + lesStatutsString[key]);
                lesStatuts.Add(int.Parse(key), DateTime.Parse(lesStatutsString[key]));

            }

            foreach (int key in lesStatuts.Keys)
            {

                Console.WriteLine(key + " : " + lesStatutsString[key];


            }

            Console.ReadKey();
        }
    }
}
