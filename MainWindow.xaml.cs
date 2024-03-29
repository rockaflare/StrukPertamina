﻿using System.Configuration;
using System.Collections.Specialized;
using System.Windows;
using System.Drawing.Printing;
using System;
using System.Printing;
using System.Text.RegularExpressions;

namespace StrukPertamina
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetPrinterList();
            DisplayStrukConfig();
        }

        string printerName = "";
        string jamtext = "";
        StrukModel strukModel = new StrukModel();
         

        private void GetPrinterList()
        {
            PrinterSelect.Items.Add("Pilih Printer..");
            String pkInstalledPrinters;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                PrinterSelect.Items.Add(pkInstalledPrinters);
            }
            PrinterSelect.SelectedIndex = 0;
        }

        private void DisplayStrukConfig()
        {
            Nama.Text = ConfigurationManager.AppSettings.Get("Nama");
            Alamat.Text = ConfigurationManager.AppSettings.Get("Alamat");
            RecNo.Text = ConfigurationManager.AppSettings.Get("RecNo");
            HoseNo.Text = ConfigurationManager.AppSettings.Get("Hose");
            Products.Text = ConfigurationManager.AppSettings.Get("Produk");
            Harga.Text = ConfigurationManager.AppSettings.Get("Harga");
            Prov.Text = ConfigurationManager.AppSettings.Get("Prov");
            Footer.Text = ConfigurationManager.AppSettings.Get("Footer");
        }

        private void JamGenerateChange()
        {
            if (Jam.SelectedValue.ToString() == "Manual")
            {
                JamManual.IsEnabled = true;
            }
            else
            {
                JamManual.Text = "";
                JamManual.IsEnabled = false;
                StrukService printService = new StrukService();
                jamtext = printService.GenerateTimeString(Jam.SelectedValue.ToString());
            }
        }

        private void SaveStrukConfig(string key, string value)
        {
            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if(settings[key] == null)
                {
                    settings.Add(key,value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {

                MessageBox.Show("Error menyimpan data");
            }
        }
        #region Events
        private void Nama_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SaveStrukConfig("Nama", Nama.Text);
        }
        private void Alamat_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SaveStrukConfig("Alamat", Alamat.Text);
        }
        private void RecNo_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SaveStrukConfig("RecNo", RecNo.Text);
        }
        //private void RecGen_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{
        //    StrukService strukService = new StrukService();
        //    int recGen = 7;
        //    if (!String.IsNullOrWhiteSpace(RecNo.Text))
        //    {
        //        if (RecGen.SelectedIndex > -1)
        //        {
        //            recGen = Int32.Parse(RecGen.Text);
        //            RecNo.Text = strukService.GenerateReceiptNumber(RecNo.Text, recGen);
        //        }
        //        else
        //        {
        //            RecNo.Text = strukService.GenerateReceiptNumber(RecNo.Text, recGen);
        //        }
        //    }
        //}
        private void HoseNo_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SaveStrukConfig("Hose", HoseNo.Text);
        }
        private void Products_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SaveStrukConfig("Produk", Products.Text);
        }
        private void Jam_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            JamGenerateChange();
        }
        private void Harga_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SaveStrukConfig("Harga", Harga.Text);
            CalculateTotal();
        }
        private void Volume_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CalculateTotal();
        }
        private void Total_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
        private void Prov_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SaveStrukConfig("Prov", Prov.Text);
        }
        private void Footer_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SaveStrukConfig("Footer", Footer.Text);
        }
        #endregion

        private void Harga_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Volume_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CalculateTotal()
        {
            if (!String.IsNullOrEmpty(Harga.Text) && !String.IsNullOrEmpty(Volume.Text))
            {
                int harga = Int32.Parse(Harga.Text);
                int volume = Int32.Parse(Volume.Text);
                string total = Convert.ToString(harga * volume);
                Total.Text = total;
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            if (PrinterSelect.SelectedIndex != 0)
            {
                try
                {
                    StrukService printService = new StrukService();
                    strukModel.Nama = Nama.Text;
                    strukModel.Alamat = Alamat.Text;
                    strukModel.ReceiptNo = String.Format("Receipt No:{0}", RecNo.Text);
                    strukModel.Hose = String.Format("Hose No:{0}", HoseNo.Text);
                    strukModel.Produk = String.Format("Product:{0}", Products.Text);
                    strukModel.Jam = String.Format("Time: {0}", jamtext);
                    strukModel.Tanggal = String.Format("Date:  {0}", Tanggal.Text);
                    strukModel.Harga = String.Format("Price:          {0}", Harga.Text);
                    strukModel.Volume = String.Format("Volume:         {0}", Volume.Text);
                    strukModel.TotalSale = String.Format("Total Sale:     {0}", Total.Text);
                    strukModel.Prov = Prov.Text;
                    strukModel.Footer = Footer.Text;
                    printService.PrintStruk(printerName, strukModel);

                    RecNo.Text = printService.GenerateReceiptNumber(RecNo.Text, MultiplierStart.Text, MultiplierEnd.Text);
                    JamGenerateChange();
                    Tanggal.SelectedDate = Tanggal.SelectedDate.Value.AddDays(1);
                    
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                MessageBox.Show("Printer belum dipilih!");
            }
        }

        private void PrinterSelect_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            printerName = PrinterSelect.SelectedValue.ToString();
        }

        private void JamManual_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            jamtext = JamManual.Text;
        }

        private void Skip_Click(object sender, RoutedEventArgs e)
        {
            StrukService printService = new StrukService();
            RecNo.Text = printService.GenerateReceiptNumber(RecNo.Text, MultiplierStart.Text, MultiplierEnd.Text);
            JamGenerateChange();
            Tanggal.SelectedDate = Tanggal.SelectedDate.Value.AddDays(1);
        }
    }
}
