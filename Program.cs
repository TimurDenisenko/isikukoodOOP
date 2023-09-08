using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace isikukood
{
    public class Program
    {

        public static void Main()
        {
            Console.ForegroundColor = ConsoleColor.White;
            IdCode idcode = new IdCode("");
            while (true)
            {
                Console.Clear();
                Console.WriteLine("                         Menu\n\n[1] Kontroll isikukood          [2] Vaata info ");
                Console.WriteLine("\n[3]");
                Console.WriteLine();
                ConsoleKeyInfo level = Console.ReadKey();
                switch (level.KeyChar)
                {
                    case '1':
                        idcode.ControlIdCode();
                        break;
                    case '2':
                        idcode.ShowInfo();
                        break;
                    default: break;
                }
                
            }
        }
    }
}
