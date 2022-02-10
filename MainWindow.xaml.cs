using System.Configuration;
using System.Collections.Specialized;
using System.Windows;
using System.Drawing.Printing;
using System;
using System.Printing;

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

        private void Nama_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SaveStrukConfig("Nama", Nama.Text);
        }
    }
}
