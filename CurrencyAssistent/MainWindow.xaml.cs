using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CurrencyAssistent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Comunicators.CurrencyDownloader.DownloadFiles();
        }

        private void BtnXlsx_Click(object sender, RoutedEventArgs e)
        {
            using (var diag = new System.Windows.Forms.SaveFileDialog())
            {
                diag.AddExtension = true;
                diag.FileName = "kurzyExcel";
                diag.Filter = "Excel (*.xlsx) |*.xlsx";
                var res = diag.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllBytes(diag.FileName, Exports.ExcelExport.ExportCurrencies());
                    }
                    catch(Exception exc)
                    {

                    }
                }
            }
        }
    }
}