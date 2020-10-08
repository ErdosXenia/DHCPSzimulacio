using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dhcpszimulacio
{
    

    class Program
    {
        static List<string> excluded = new List<string>();

        static void BeolvasExcluded()
        {
            
            try
            {
                StreamReader file = new StreamReader("excluded.csv");
                try
                {
                    while (!file.EndOfStream)
                    {
                        excluded.Add(file.ReadLine());
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    
                }
                finally
                {
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        static string CimEgyenlo(string cim)
        {
            //szétvágni "."
            //az utolsót int-é konvertálni
            //egyet hozzáadni (255-öt ne lépjük túl)
            //összefűzni string-é

            string[] adatok = cim.Split('.');
            int okt4 = Convert.ToInt32(adatok[3]);
            if (okt4 < 255)
            {
                okt4++;
            }
            return adatok[0] + "." + adatok[1] + "." + adatok[2] + "." + okt4.ToString();

        }

        static void Main(string[] args)
        {
            BeolvasExcluded();
            //foreach (var e in excluded)
            //{
            //    Console.WriteLine(e);
            //}

            

            Console.WriteLine("\nVége...");
            Console.ReadKey();
        }
    }
}
