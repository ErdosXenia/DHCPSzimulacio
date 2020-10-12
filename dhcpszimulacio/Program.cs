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
                string[] a = parancs.Split(';');
                string mac = a[1];

                if (dhcp.ContainsKey(mac))
                {
                    Console.WriteLine($"DHCP {mac} ---> {dhcp[mac]}");
                }
                else
                {
                    if (reserved.ContainsKey(mac))
                    {
                        Console.WriteLine($"Reserved {mac} --> {reserved[mac]}");
                        dhcp.Add(mac, reserved[mac]);
                    }
                    else
                    {
                        string indulo = "192.168.10.100";
                        int okt4 = 100;

                        while (okt4 < 200 && (dhcp.ContainsValue(indulo) 
                               || reserved.ContainsValue(indulo) 
                               || excluded.Contains(indulo)))
                        {
                            okt4++;
                            indulo = CimEgyenlo(indulo);
                        }
                        if (okt4 < 200)
                        {
                            Console.WriteLine($"Kiosztott {mac} ---> {indulo}");
                            dhcp.Add(mac, indulo);
                        }
                        else
                        {
                            Console.WriteLine($"{mac} nincs IP!");
                        }

                    }
                }
            }
            else
            {
                //parancs = "release;192.168.10.101"
                string[] a = parancs.Split(';');
                string ipcim = a[1];
                //excluded listából ha van törölni
                if (excluded.Contains(ipcim))
                {
                    excluded.Remove(ipcim);
                    Console.WriteLine($"Release excluded-ból: {ipcim}");
                }

                //van-e a reserved dictionary-ba
                if (reserved.ContainsValue(ipcim))
                {
                    Console.WriteLine($"Release reserved-ből: {ipcim}");
                    string mac = "";
                    foreach (var r in reserved)
                    {
                        if (r.Value == ipcim)
                        {
                            mac = r.Key;
                        }
                    }
                    reserved.Remove(mac);
                    
                }

                if (dhcp.ContainsValue(ipcim))
                {
                    Console.WriteLine($"Release dhcp-ből: {ipcim}");
                    string mac1 = "";
                    foreach (var d in dhcp)
                    {
                        if (d.Value == ipcim)
                        {
                            mac1 = d.Key;
                        }
                    }
                    dhcp.Remove(mac1);
                }
            }
        }

        static void FajlbIras()
        {
            StreamWriter sw = new StreamWriter("dhcp_kesz.csv");
            foreach (var d in dhcp)
            {
                sw.WriteLine(d.Key+"; "+d.Value);
            }  
            sw.Close();
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


            Feladatok();
            FajlbIras();
            
            Console.WriteLine("\nVége...");
            Console.ReadKey();
        }
    }
}
