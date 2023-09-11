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
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nVajutage Enter...");
            Console.ReadLine();
            return returndata;
        }

        public string CheckGender(IdCode idcode)
        {
            int gendernum = idcode.GetGenderNumber();
            double result = gendernum % 2;
            if (result==0)
            {
                return "Tüdruk";
            }
            else
            {
                return "Poiss";
            }
        }

        public string CheckBirth(IdCode idcode)
        {
            List<char> idcodeList = idcode._idCode.ToList();
            string HaiglaNum = idcodeList[7].ToString() + idcodeList[8].ToString() + idcodeList[9].ToString();
            int HaiglaNumInt = Convert.ToInt32(HaiglaNum);
            string haigla="";
            if (HaiglaNumInt >= 1 && HaiglaNumInt < 11)
            {
                haigla = "Kuressaare Haigla";
            }
            else if (HaiglaNumInt >= 11 && HaiglaNumInt < 20)
            {
                haigla = "Tartu Ülikooli Naistekliinik, Tartumaa, Tartu";
            }
            else if (HaiglaNumInt >= 20 && HaiglaNumInt < 221)
            {
                haigla = "Ida-Tallinna Keskhaigla, Pelgulinna sünnitusmaja, Hiiumaa, Keila, Rapla haigla, Loksa haigla";
            }
            else if (HaiglaNumInt >= 221 && HaiglaNumInt < 271)
            {
                haigla = "Ida-Viru Keskhaigla (Kohtla-Järve, endine Jõhvi)";
            }
            else if (HaiglaNumInt >= 271 && HaiglaNumInt < 371)
            {
                haigla = "Maarjamõisa Kliinikum (Tartu), Jõgeva Haigla";
            }
            else if (HaiglaNumInt >= 371 && HaiglaNumInt < 421)
            {
                haigla = "Narva Haigla";
            }
            else if (HaiglaNumInt >= 421 && HaiglaNumInt < 471)
            {
                haigla = "Pärnu Haigla";
            }
            else if (HaiglaNumInt >= 471 && HaiglaNumInt < 491)
            {
                haigla = "Pelgulinna Sünnitusmaja (Tallinn), Haapsalu haigla";
            }
            else if (HaiglaNumInt >= 491 && HaiglaNumInt < 521)
            {
                haigla = "Järvamaa Haigla (Paide)";
            }
            else if (HaiglaNumInt >= 521 && HaiglaNumInt < 571)
            {
                haigla = "Rakvere, Tapa haigla";
            }
            else if (HaiglaNumInt >= 571 && HaiglaNumInt < 601)
            {
                haigla = "Valga Haigla";
            }
            else if (HaiglaNumInt >= 601 && HaiglaNumInt < 651)
            {
                haigla = "Viljandi Haigla";
            }
            else if (HaiglaNumInt >= 651 && HaiglaNumInt < 701)
            {
                haigla = "Lõuna-Eesti Haigla (Võru), Põlva Haigla";
            }
            return haigla;
            
        }

        public string GetBirthDateFull(IdCode idcode)
        {
            string birthday = idcode.GetBirthDate().ToString();    
            string month = "";
            List<string> birthdayInList = birthday.Split('.').ToList();
            switch (birthdayInList[1])
            {
                case "01": month = "Jaanuar"; break;
                case "02": month = "Veebruar"; break;
                case "03": month = "Märts"; break;
                case "04": month = "Aprill"; break;
                case "05": month = "Mai"; break;
                case "06": month = "Juuni"; break;
                case "07": month = "Juuli"; break;
                case "08": month = "August"; break;
                case "09": month = "September"; break;
                case "10": month = "Oktoober"; break;
                case "11": month = "November"; break;
                case "12": month = "Detsember"; break;
            }
            return birthdayInList[0] +" "+ month + ", " + birthdayInList[2]+" a.";
        }

        public string ZodiacSign(IdCode idcode)
        {
            string birthday = idcode.GetBirthDate().ToString();
            List<string> birthdayInList = birthday.Split('.').ToList();
            string zodiac="";
            /*
             [0] - день
             [1] - месяц
             [2] - год
             */
            if (Convert.ToInt32(birthdayInList[1])==3 && Convert.ToInt32(birthdayInList[0])>=21 || Convert.ToInt32(birthdayInList[1]) == 4 && Convert.ToInt32(birthdayInList[0]) <= 20)
            {
                zodiac = "Jäär";
            }
            else if (Convert.ToInt32(birthdayInList[1]) == 4 && Convert.ToInt32(birthdayInList[0]) >= 21 || Convert.ToInt32(birthdayInList[1]) == 5 && Convert.ToInt32(birthdayInList[0]) <= 20)
            {
                zodiac = "Sõnn";
            }
            else if (Convert.ToInt32(birthdayInList[1]) == 5 && Convert.ToInt32(birthdayInList[0]) >= 21 || Convert.ToInt32(birthdayInList[1]) == 6 && Convert.ToInt32(birthdayInList[0]) <= 21)
            {
                zodiac = "Kaksikud";
            }
            else if (Convert.ToInt32(birthdayInList[1]) == 6 && Convert.ToInt32(birthdayInList[0]) >= 22 || Convert.ToInt32(birthdayInList[1]) == 7 && Convert.ToInt32(birthdayInList[0]) <= 22)
            {
                zodiac = "Vähk";
            }
            else if (Convert.ToInt32(birthdayInList[1]) == 7 && Convert.ToInt32(birthdayInList[0]) >= 23 || Convert.ToInt32(birthdayInList[1]) == 8 && Convert.ToInt32(birthdayInList[0]) <= 23)
            {
                zodiac = "lõvi";
            }
            else if (Convert.ToInt32(birthdayInList[1]) == 8 && Convert.ToInt32(birthdayInList[0]) >= 24 || Convert.ToInt32(birthdayInList[1]) == 9 && Convert.ToInt32(birthdayInList[0]) <= 23)
            {
                zodiac = "Neitsi";
            }
            else if (Convert.ToInt32(birthdayInList[1]) == 9 && Convert.ToInt32(birthdayInList[0]) >= 24 || Convert.ToInt32(birthdayInList[1]) == 10 && Convert.ToInt32(birthdayInList[0]) <= 23)
            {
                zodiac = "Kaalud";
            }
            else if (Convert.ToInt32(birthdayInList[1]) == 10 && Convert.ToInt32(birthdayInList[0]) >= 24 || Convert.ToInt32(birthdayInList[1]) == 11 && Convert.ToInt32(birthdayInList[0]) <= 22)
            {
                zodiac = "Skorpion";
            }
            else if (Convert.ToInt32(birthdayInList[1]) == 11 && Convert.ToInt32(birthdayInList[0]) >= 23 || Convert.ToInt32(birthdayInList[1]) == 12 && Convert.ToInt32(birthdayInList[0]) <= 21)
            {
                zodiac = "Ambur";
            }
            else if (Convert.ToInt32(birthdayInList[1]) == 12 && Convert.ToInt32(birthdayInList[0]) >= 22 || Convert.ToInt32(birthdayInList[1]) == 1 && Convert.ToInt32(birthdayInList[0]) <= 20)
            {
                zodiac = "Kaljukits";
            }
            else if (Convert.ToInt32(birthdayInList[1]) == 1 && Convert.ToInt32(birthdayInList[0]) >= 21 || Convert.ToInt32(birthdayInList[1]) == 2 && Convert.ToInt32(birthdayInList[0]) <= 20)
            {
                zodiac = "Veevalaja";
            }
            else if (Convert.ToInt32(birthdayInList[1]) == 2 && Convert.ToInt32(birthdayInList[0]) >= 21 || Convert.ToInt32(birthdayInList[1]) == 3 && Convert.ToInt32(birthdayInList[0]) <= 20)
            {
                zodiac = "Kala";
            }
            return zodiac;
        }

        public string TalismanStone(IdCode idcode)
        {
            string birthday = idcode.GetBirthDate().ToString().Replace('.',' ').Replace(" ","");
            int sum=0;
            do
            {
                foreach (char item in birthday)
                {
                    sum += Convert.ToInt32(item.ToString());
                }
                birthday = Convert.ToString(sum);
                sum = 0;
            }
            while (birthday.Length>=2);
            string stone = "";
            switch (Convert.ToInt32(birthday))
            {
                case 1:
                    stone ="Selged, selged kivid nagu aventuriin või karneool. Need mineraalid annavad moraalse ja füüsilise harmoonia, naudingu ja tervise."; break;
                case 2:
                    stone ="Parim on pöörata tähelepanu kuukivile ja pärlitele. Teil on sageli halvad harjumused ja need kalliskivid aitavad teil neist üle saada."; break;
                case 3:
                    stone ="Peridoot, peridoot ja türkiis on kõige sobivamad. Nende kalliskividega talismanid mitte ainult ei kaunista toodet, vaid aitavad kaitsta kurjade loitsude eest, saavutavad rahalise sõltumatuse ning saavutavad ühiskonnas soovitud koha ja austuse."; break;
                case 4:
                    stone ="Tasub meeles pidada safiiri, krüsopraasi ja jade. Safiir on aususe ja kavatsuste puhtuse sümbol, selline talisman aitab teil ära tunda teiste valesid ja kadedust. Krüsopraas ja jade köidavad teid huvitava inimese tähelepanu ja annavad teile tervise."; break;
                case 5:
                    stone ="Kõik smaragdisordid, aga ka tiigrisilm ja tsirkoon vastavad. Need mineraalid soodustavad tarkuse ja tähelepanelikkuse arengut. Need aitavad parandada teie rahalist olukorda ja kaitsta teid negatiivsete mõtete eest."; break;
                case 6: 
                    stone ="Teemandid - tervise, igavese elu, tarkuse ja harmoonia talisman."; break;
                case 7:
                    stone ="Ideaalsed kivid teile on rubiin ja granaat. Kõikide punaste toonide kalliskivid on amuletid, mis mõjutavad sündides antud andeid ja iseloomuomadusi."; break;
                case 8:
                    stone ="Oonüksi, ahhaati, jeti ja kvartsi eelistamine. Mineraalid aitavad arendada julgust, enesekindlust ning säilitada ka mõtete selgust ja mõtete puhtust."; break;
                case 9:
                    stone = "Parem on osta mäekristalli, roosa kvartsi, morioni ja muude vääriliste kvartsisortide ehteid. Need amuletid mõjutavad kõike salajast ja intiimset, õpetades nende omanikku paljastama teiste tõelisi mõtteid ja soove."; break;
            }
            return stone;

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
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            Console.WriteLine("\n");
            string gender = idcode.CheckGender(idcode);
            if (gender=="Poiss")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(gender);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(gender);
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(idcode.GetBirthDateFull(idcode));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(CheckBirth(idcode));
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Tähtkuju: "+ZodiacSign(idcode));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Talismani kivi: "+TalismanStone(idcode));
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nVajutage Enter...");
            Console.ReadLine();
        }

        public List<IdCode> AddIdCode(List<IdCode> idCodes)
        {
            Console.Clear();
            IdCode idcode;
            Console.Write("Kirjuta sinu isikukood: ");
            string id = Console.ReadLine();
            idcode = new IdCode(id);
            foreach (IdCode item in idCodes)
            {
                if (item._idCode==id)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Vale isikukood!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\nVajutage Enter...");
                    Console.ReadLine();
                    return idCodes;
                }
            }
            if (idcode.IsValid())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Kõik on korras!");
                Console.ForegroundColor = ConsoleColor.White;
                idCodes.Add(idcode);
                Console.Write("\nVajutage Enter...");
                Console.ReadLine();
                return idCodes;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Vale isikukood!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\nVajutage Enter...");
                Console.ReadLine();
                return idCodes;
            }
            
        }

        public void ShowAllIdCode(List<IdCode> idCodes)
        {
            Console.Clear();
            foreach (IdCode item in idCodes)
            {
                Console.WriteLine(item._idCode+"; ");
            }
            Console.Write("\nVajutage Enter...");
            Console.ReadLine();
        }
    }
}
