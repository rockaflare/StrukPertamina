using System.Configuration;
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
            Products.Text = ConfigurationManager.AppSettings.Get("DEXLITE");
            Harga.Text = ConfigurationManager.AppSettings.Get("Harga");
            Prov.Text = ConfigurationManager.AppSettings.Get("Prov");
            Footer.Text = ConfigurationManager.AppSettings.Get("Footer");
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
        private void RecGen_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
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

        }
        private void Harga_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SaveStrukConfig("Harga", Harga.Text);
            Total.Text = Int32.TryParse(Harga.Text, out int n) ? Convert.ToString(Int32.Parse(Volume.Text) * Int32.Parse(Harga.Text)) : "";
        }
        private void Volume_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Total.Text = Convert.ToString(Int32.Parse(Volume.Text) * Int32.Parse(Harga.Text));
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
    }
}
