using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrukPertamina
{
    public class StrukService
    {
        public string GenerateTimeString(string time)
        {
            Random random = new Random();
            string timeHour = time;
            int timeMinute = random.Next(10,60);
            int timeSecond = random.Next(10, 60);
            string timeString = String.Format("{0}:{1}:{2}", timeHour, timeMinute, timeSecond);

            return timeString;
        }

        public string GenerateReceiptNumber(string recDefault, int multiplier)
        {
            int receipt = Int32.Parse(recDefault) + multiplier;
            string newReceiptNum = Convert.ToString(receipt).PadLeft(12, '0');

            return newReceiptNum;
        }
    }
}
