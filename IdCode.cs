using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isikukood
{
    public class IdCode
    {
        public string _idCode { get; }

        public IdCode(string idCode)
        {
            _idCode = idCode;
        }

        private bool IsValidLength()
        {
            return _idCode.Length == 11;
        }

        private bool ContainsOnlyNumbers()
        {
            // return _idCode.All(Char.IsDigit);

            for (int i = 0; i < _idCode.Length; i++)
            {
                if (!Char.IsDigit(_idCode[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private int GetGenderNumber()
        {
            return Convert.ToInt32(_idCode.Substring(0, 1));
        }

        private bool IsValidGenderNumber()
        {
            int genderNumber = GetGenderNumber();
            return genderNumber > 0 && genderNumber < 7;
        }

        private int Get2DigitYear()
        {
            return Convert.ToInt32(_idCode.Substring(1, 2));
        }

        public int GetFullYear()
        {
            int genderNumber = GetGenderNumber();
            // 1, 2 => 18xx
            // 3, 4 => 19xx
            // 5, 6 => 20xx
            return 1800 + (genderNumber - 1) / 2 * 100 + Get2DigitYear();
        }

        private int GetMonth()
        {
            return Convert.ToInt32(_idCode.Substring(3, 2));
        }

        private bool IsValidMonth()
        {
            int month = GetMonth();
            return month > 0 && month < 13;
        }

        private static bool IsLeapYear(int year)
        {
            return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
        }
        private int GetDay()
        {
            return Convert.ToInt32(_idCode.Substring(5, 2));
        }

        private bool IsValidDay()
        {
            int day = GetDay();
            int month = GetMonth();
            int maxDays = 31;
            if (new List<int> { 4, 6, 9, 11 }.Contains(month))
            {
                maxDays = 30;
            }
            if (month == 2)
            {
                if (IsLeapYear(GetFullYear()))
                {
                    maxDays = 29;
                }
                else
                {
                    maxDays = 28;
                }
            }
            return 0 < day && day <= maxDays;
        }

        private int CalculateControlNumberWithWeights(int[] weights)
        {
            int total = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                total += Convert.ToInt32(_idCode.Substring(i, 1)) * weights[i];
            }
            return total;
        }

        private bool IsValidControlNumber()
        {
            int controlNumber = Convert.ToInt32(_idCode[^1..]);
            int[] weights = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 };
            int total = CalculateControlNumberWithWeights(weights);
            if (total % 11 < 10)
            {
                return total % 11 == controlNumber;
            }
            // second round
            int[] weights2 = { 3, 4, 5, 6, 7, 8, 9, 1, 2, 3 };
            total = CalculateControlNumberWithWeights(weights2);
            if (total % 11 < 10)
            {
                return total % 11 == controlNumber;
            }
            // third round, control number has to be 0
            return controlNumber == 0;
        }

        public bool IsValid()
        {
            return IsValidLength() && ContainsOnlyNumbers()
                && IsValidGenderNumber() && IsValidMonth()
                && IsValidDay()
                && IsValidControlNumber();
        }

        public DateOnly GetBirthDate()
        {
            int day = GetDay();
            int month = GetMonth();
            int year = GetFullYear();
            return new DateOnly(year, month, day);
        }

        public bool ControlIdCode()
        {
            Console.Clear();
            string id;
            bool returndata;
            IdCode idcode;
            Console.Write("Kirjuta sinu isikukood: ");
            id = Console.ReadLine();
            idcode = new IdCode(id);
            if (idcode.IsValid())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("See isikukood on olemas");
                returndata = true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Vale isikukood!");
                returndata = false;
            }
            Console.ForegroundColor=ConsoleColor.White;
            Console.Write("\nVajutage Enter...");
            Console.ReadLine();
            return returndata;
        }

        public string CheckGender(IdCode idcode)
        {
            string stringidcode = idcode.ToString();
            int intidcode = Convert.ToInt32(stringidcode[0]);
            double result = intidcode % 2;
            if (result==0)
            {
                return "Tüdruk";
            }
            else
            {
                return "Poiss";
            }
        }

        public void ShowInfo()
        {
            Console.Clear();
            string id;
            IdCode idcode;
            Console.Write("Kirjuta sinu isikukood: ");
            id = Console.ReadLine();
            idcode = new IdCode(id);
            if (idcode.IsValid())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("See isikukood on olemas");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Vale isikukood!");
                return;
            }
            Console.ForegroundColor=ConsoleColor.White;
            Console.WriteLine("Sünnipäev: "+idcode.GetBirthDate());
            Console.WriteLine("Sugu: "+idcode.CheckGender(idcode)) ;
            Console.Write("\nVajutage Enter...");
            Console.ReadLine();
        }
    }
}
