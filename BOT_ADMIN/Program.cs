using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace BOT_ADMIN
{
    class Program
    {
        ///<summary> Recuerda después de compilar mover el Release de 'BOT_IRC' a una carpeta
        ///con ese mismo nombre a donde vayas instalar el 'BOT_ADMIN', y cambiar el boolean
        ///en el método 'RunBOT' de 'true' a 'false'</summary>

        static void Main(string[] args)
        {
            try
            {
                Restart:
                Console.Clear();
                //Console.WriteLine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory));
                Console.WriteLine("Cuantos Bots Quieres Abrir?");
                string strResult = Console.ReadLine();

                
                int intResult = 0;
                if (int.TryParse(strResult, out intResult))
                {
                    for (int i = 0; i < intResult; i++)
                    {
                        Thread t = new Thread(RunBOT);
                        t.Start();
                    }
                    Console.WriteLine("Success");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine(":P You must input a number, JUST NUMBERS!!!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    goto Restart;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due To: " + ex.Message);
                Console.ReadLine();
            }
        }

        public static void RunBOT()
        {
            //C:\\Users\\nicol\\Desktop\\BOT_IRC\\BOT_ADMIN\\bin\\Debug"
            bool isTest = true;
            string PathToApp = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\"));
            if (isTest)
            {
                PathToApp = Path.Combine(PathToApp, @"BOT_IRC\bin\Release\BOT_IRC.exe");
            }
            else
            {
                if(!Directory.Exists(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\BOT_IRC"))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\BOT_IRC");
                }

                PathToApp = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\BOT_IRC\\BOT_IRC.exe";
            }
            System.Diagnostics.Process.Start(PathToApp);
        }
    }
}
