using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Win32;
using System.Windows.Forms;

namespace ZeroPlan
{
    class Program
    {
        static PowerStatus status = SystemInformation.PowerStatus; //so that this shit can be accessed anywhere
        static Process powercfg = new Process(); //same
        static string currentPlan; //tbh this is terrible code but how else??

        static void Main(string[] args)
        {
            powercfg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; //no console window
            //powercfg.StartInfo.RedirectStandardOutput = true; //powercfg output is supposed to go into this app's console
            //powercfg.StartInfo.UseShellExecute = false; //i have no idea what this means
            powercfg.StartInfo.FileName = @"powercfg.exe"; //picking powercfg itself

            //http://software.intel.com/en-us/articles/net-monitor-power-status/
            //I HAVE NO IDEA WHAT I AM DOING
            while (true)
            {
                SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
                System.Threading.Thread.Sleep(600000); //you know, i have no idea why this works. either way, it'll tick once an hour. dunno why!
                //Console.WriteLine("Tick!");
            }


            //test shit
            /*Console.ReadKey();
            ChangePlan("Power Saver", powercfg);
            Console.ReadKey();
            ChangePlan("Balanced", powercfg); 
            Console.ReadKey();
            ChangePlan("High Power", powercfg);
            Console.WriteLine("Test");
            Console.ReadKey();*/

        }
        static void ChangePlan(string name)
        {
            if (name == currentPlan) //so that we don't switch the plan multiple times, because that is stupid
            {
                return;
            }

            switch (name)
            {
                case "Power Saver":
                    powercfg.StartInfo.Arguments = "-setactive a1841308-3541-4fab-bc81-f71556f20b4a";
                    //Console.WriteLine("Selecting Power Saver");
                    currentPlan = "Power Saver";
                    break;
                case "Balanced": //i know it's not used anywhere but here it is anyway
                    powercfg.StartInfo.Arguments = "-setactive 381b4222-f694-41f0-9685-ff5bb260df2e";
                    //Console.WriteLine("Selecting Balanced...");
                    currentPlan = "Balanced";
                    break;
                case "High Power":
                    powercfg.StartInfo.Arguments = "-setactive 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c";
                    //Console.WriteLine("Selecting High Power...");
                    currentPlan = "High Power";
                    break;
                default:
                    //Console.WriteLine("Invalid power plan.");
                    return;
            }
            powercfg.Start();
        }

        //http://stackoverflow.com/questions/17832969/c-constantly-monitor-battery-level
        static void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == Microsoft.Win32.PowerModes.StatusChange)
            {
                //Console.WriteLine("Status change detected...");
                switch (status.PowerLineStatus)
                {
                    case PowerLineStatus.Online:
                        //Console.WriteLine("We're online...");
                        ChangePlan("High Power");
                        break;
                    case PowerLineStatus.Offline:
                        //Console.WriteLine("We're offline...");
                        ChangePlan("Power Saver");
                        break;
                    default:
                        //Console.WriteLine("what");
                        break;
                }
            }
        }

    }
}
