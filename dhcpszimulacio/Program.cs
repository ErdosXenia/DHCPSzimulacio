﻿using System;
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
        static Dictionary<string, string> dhcp = new Dictionary<string, string>();
        static Dictionary<string, string> reserved = new Dictionary<string, string>();
        static List<string> commands = new List<string>();

        static void BeolvasList(List<string> l, string filename)
        {
            
            try
            {
                StreamReader file = new StreamReader(filename);
                try
                {
                    while (!file.EndOfStream)
                    {
                        l.Add(file.ReadLine());
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

        static void BeolvasDictionary(Dictionary<string, string> d, string filenev)
        {
            try
            {
                StreamReader file = new StreamReader(filenev);
                while (!file.EndOfStream)
                {
                    string[] adatok = file.ReadLine().Split(';');
                    d.Add(adatok[0], adatok[1]);
                }
                file.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void Feladat(string parancs)
        {
            /*parancs = "request; 123DKK345LJ"
             * először csak a  "request" paracsal foglalkozunk
             * x Megnézzük hogy "request"
             * ki kell szedni a mac címet a parancsból
             
             */
            if (parancs.Contains("request"))
            {
                Console.WriteLine("Ez oké");
            }
            else
            {
                Console.WriteLine("Nem oké");
            }
        }

        static void Feladatok()
        {
            foreach (var command in commands)
            {
                Feladat(command);
            }
        }

        static void Main(string[] args)
        {
            BeolvasList(excluded, "excluded.csv");
            BeolvasList(commands, "test.csv");

            BeolvasDictionary(dhcp, "dhcp.csv");
            BeolvasDictionary(reserved, "reserved.csv");


            Feladat("request;Ebben nincs semmi");
            
            Console.WriteLine("\nVége...");
            Console.ReadKey();
        }
    }
}
