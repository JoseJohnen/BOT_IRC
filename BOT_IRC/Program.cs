
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BOT_IRC
{
    class Program
    {
        public static List<Bot> l_bots = new List<Bot>();

        private static void Main(string[] args)
        {
            CreateBot();

            foreach (Bot bot in l_bots)
            {
                bot.WorkingBot();
            }
        }

        public static bool CreateBot()
        {
            try
            {
                Restart:
                Console.Clear();
                Console.WriteLine("How many robots do you want to create?");
                string strResponse = Console.ReadLine();

                int intResult = 0;
                if (int.TryParse(strResponse, out intResult))
                {
                    Console.WriteLine("¡Buenops! :D");
                    //for (int i = 0; i < intResult; i++)
                    //{
                        l_bots.Add(new Bot("chat.freenode.net", ("ClapTrakaLaKa" + l_bots.Count), "#locos"));
                        l_bots.Add(new Bot("irc.arstechnica.com", ("ClapTrakaLaKa" + l_bots.Count), "#locos"));
                        Console.WriteLine("Bot N° " + l_bots[0].nickname + " Creado con exito! :D");
                        Console.WriteLine("Bot N° " + l_bots[1].nickname + " Creado con exito! :D");
                    //}
                }
                else
                {
                    Console.WriteLine(":P You must input a number, JUST NUMBERS!!!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    goto Restart;
                }
                return true;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
                return false;
            }
             
        }

    }
}

// The End ?