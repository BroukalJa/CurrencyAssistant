using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;


namespace CurrencyAssistent
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        Timer checkForTime;

        const double interval60Minutes = 60 * 60 * 1000; // milliseconds to one hour

        const double interval1Day = interval60Minutes * 24; // milliseconds to one day

        const double interval16Hours = interval60Minutes * 16;

        

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DataParsers.Parsers.CurrencyParser();
            IsolatedStorageWorkers.IsolatedStorageWorker.LoadIS();
            Comunicators.CurrencyDownloader.DownloadFiles();
            var time = DateTime.Now.TimeOfDay.TotalMilliseconds;
            double interval = 0;
            if (time < interval16Hours)
                interval = interval16Hours - time;
            else
                interval = interval1Day - time + interval16Hours;
            checkForTime = new Timer(interval);
            checkForTime.Elapsed += CheckForTime_Elapsed;
            checkForTime.Enabled = true;
        }

        private void CheckForTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            (sender as Timer).Interval = interval1Day;
            (sender as Timer).Enabled = true;
            Comunicators.CurrencyDownloader.DownloadFiles();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            IsolatedStorageWorkers.IsolatedStorageWorker.UlozIS();
        }
    }
}
