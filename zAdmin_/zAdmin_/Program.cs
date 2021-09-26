using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace zAdmin_
{
    class Program
    {
        static List<string> commands = new List<string>();

        #region - DLLImport -
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        #endregion

        static void Main(string[] args)
        {
            #region - Console Awake -
            Console.Title = "zAdmin Control Panel";
            //Console.WriteLine("zAdmin, Take Over The Control");

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);
            #endregion

            #region - Register Commands -
            AddCommand("help");
            AddCommand("author");
            AddCommand("exit");
            AddCommand("openPath");
            AddCommand("process.kill");
            AddCommand("system.shutdown");
            AddCommand("system.restart");
            AddCommand("system.logout");
            #endregion

            while (true)
            {
                Console.Write("\nzAdmin>");
                string prompt = Console.ReadLine();

                #region kidolgozandó
                /*for (int i = 0; i < commands.Count; i++)
                {
                    if (prompt == commands[i])
                    {
                        Program program = new Program();
                        program.GetType().GetMethod(prompt).Invoke(program, null);
                    }
                }*/
                #endregion

                switch (prompt)
                {
                    case "help": case "?":
                        help();
                        break;
                    case "author":
                        Author.ShowAuthor();
                        break;

                    case "exit":
                        zAdmin.Exit();
                        break;

                    case "openPath":
                        openPath();
                        break;
                    case "process.kill":
                        processKill();
                        break;

                    case "system.shutdown":
                        Windows.Shutdown();
                        break;
                    case "system.restart":
                        Windows.Restart();
                        break;
                    case "system.logout":
                        Windows.LogOut();
                        break;

                    default:
                        print("The command is invalid.");
                        //Console.Beep();
                        break;
                }

                //Console.WriteLine(prompt);
            }
        }        

        #region - Commands -
        static void help()
        {
            foreach (var item in commands)
            {
                printN(item + " | ");
            }
        }
        static void openPath()
        {
            printN("Path: ");
            string path = Console.ReadLine();
            
            try
            {
                Process.Start(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        static void processKill()
        {
            printN("Process name: ");
            string processName = Console.ReadLine();

            try
            {
                foreach (var item in Process.GetProcessesByName(processName))
                {
                    item.Kill();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        class zAdmin
        {
            public static void Exit()
            {
                /*foreach (var item in Process.GetProcessesByName("zAdmin_"))
                {
                    item.Kill();
                }*/
                Environment.Exit(-1);
            }
        }
        class Windows
        {
            public static void Restart()
            {
                StartShutDown("-f -r -t 5");
            }

            public static void LogOut()
            {
                StartShutDown("-l");
            }

            public static void Shutdown()
            {
                StartShutDown("-f -s -t 5");
            }

            private static void StartShutDown(string param)
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.FileName = "cmd";
                proc.WindowStyle = ProcessWindowStyle.Hidden;
                proc.Arguments = "/C shutdown " + param;
                Process.Start(proc);
            }
        }
        class Processes
        {

        }
        class Author
        {
            public static string name = "Mohamed Ziad";

            public static void ShowAuthor()
            {
                printN($"This application was created by {name}.");
            }
        }
        
        #endregion

        static void AddCommand(string name)
        {
            commands.Add(name);
        }

        static void printN(string message)
        {
            for (int i = 0; i < message.Length; i++)
            {
                Console.Write(message[i]);
                System.Threading.Thread.Sleep(25);
            }
        }
        static void print(string message)
        {
            for (int i = 0; i < message.Length; i++)
            {
                Console.Write(message[i]);
                System.Threading.Thread.Sleep(25);
            }
            Console.WriteLine();
        }
        static void Sleep(int x)
        {
            System.Threading.Thread.Sleep(x);
        }
    }
}
