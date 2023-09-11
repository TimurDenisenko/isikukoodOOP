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
            List<IdCode> idcodes = new List<IdCode>();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("                         Menu\n\n[1] Kontroll isikukood          [2] Vaata info ");
                Console.WriteLine("[3] Loo isikukood               [4] Vaata kõiki issikukoode");
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
                    case '3':
                        idcodes=idcode.AddIdCode(idcodes);
                        break;
                    case '4':
                        idcode.ShowAllIdCode(idcodes);
                        break;
                    default: break;
                }
                
            }
        }
    }
}
