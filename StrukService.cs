using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESC_POS_USB_NET.Enums;
using ESC_POS_USB_NET.Printer;

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

        public string GenerateReceiptNumber(string recDefault, string multiplierstart, string multiplierend)
        {
            Random random = new Random();
            int multiplier = 14;
            if (!string.IsNullOrWhiteSpace(multiplierstart) && !string.IsNullOrWhiteSpace(multiplierend))
            {
                multiplier = random.Next(Int32.Parse(multiplierstart), Int32.Parse(multiplierend));
            }
            long receipt = Int64.Parse(recDefault) + multiplier;
            string newReceiptNum = Convert.ToString(receipt).PadLeft(12, '0');

            return newReceiptNum;
        }

        public void PrintStruk(string printerName, StrukModel strukModel)
        {
            Printer printer = new Printer(printerName);
            EncodingProvider ppp = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
            printer.NewLines(3);
            printer.SetLineHeight(24);
            printer.Append(strukModel.Nama);
            printer.Append(strukModel.Alamat);
            printer.NewLine();
            printer.Append(strukModel.ReceiptNo);
            printer.Append(strukModel.Hose);
            printer.Append(strukModel.Produk);
            printer.NewLine();
            printer.Append(strukModel.Jam);
            printer.Append(strukModel.Tanggal);
            printer.Append(strukModel.Harga);
            printer.Append(strukModel.Volume);
            printer.Append(strukModel.TotalSale);
            printer.NewLine();
            printer.Append(strukModel.Prov);
            printer.Append(strukModel.Footer);
            printer.SetLineHeight(40);
            printer.NewLines(4);
            printer.PrintDocument();
        }

        public void PrintDemo(string printerName)
        {
            Printer printer = new Printer(printerName);
            EncodingProvider ppp = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
            printer.TestPrinter();
            printer.PrintDocument();
        }
    }
}
