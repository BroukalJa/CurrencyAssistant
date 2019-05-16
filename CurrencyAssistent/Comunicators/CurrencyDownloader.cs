using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAssistent.Comunicators
{
    public static class CurrencyDownloader
    {
        public const string CSOB = @"https://www.csob.cz/portal/lide/kurzovni-listek/-/date/kurzy.txt";
        public const string RB = @"https://www.kurzy.cz/kurzy-men/kurzovni-listek/raiffeisenbank/";
        public const string KB = @"https://api.kb.cz/openapi/v1/exchange-rates";
        public const string SPORITELNA = @"https://www.kurzy.cz/kurzy-men/kurzovni-listek/ceska-sporitelna/";
        public const string CNB = @"https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";

        public static void DownloadFiles()
        {
            CurrencySingleton.Instance.DownloadRunning = true;
            GetCurrencyFile(CSOB, "1.txt");
            GetCurrencyFile(RB, "2.txt");
            GetCurrencyFile(KB, "3.json");
            GetCurrencyFile(SPORITELNA, "4.txt");
            GetCurrencyFile(CNB, "5.txt");
        }

        public static string GetFileFolder
        {
            get
            {
                if (!Directory.Exists(Environment.CurrentDirectory + "\\Static"))
                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\Static");
                return Environment.CurrentDirectory + "\\Static\\";
            }
        }

        public static void GetCurrencyFile(string path, string file)
        {
            using (WebClient wc = new WebClient())
            {
                //wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.BaseAddress = path;
                wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                wc.DownloadFileAsync(new System.Uri(path), GetFileFolder + file);
            }
        }
        private static int finishedDownloads = 0;
        private static void Wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null && !e.Cancelled && sender is WebClient)
            {
                var wc = sender as WebClient;
                switch (wc.BaseAddress)
                {
                    case CSOB:
                        DataParsers.Parsers.CSOBParser();
                        break;
                    case KB:
                        DataParsers.Parsers.KBParser();
                        break;
                    case CNB:
                        DataParsers.Parsers.CNBParser();
                        break;
                    case SPORITELNA:
                        DataParsers.Parsers.SporitelnaParser();
                        break;
                    case RB:
                        DataParsers.Parsers.RBParser();
                        break;
                }
            }
            finishedDownloads++;
            if (finishedDownloads == 5)
            {
                finishedDownloads = 0;
                DataParsers.Parsers.CurrencyParser();
                DataParsers.Parsers.CurrencyArchiver();
                DataParsers.Parsers.DeleteSourceFiles();
                CurrencySingleton.Instance.FilterCurrencies();
                CurrencySingleton.Instance.DownloadRunning = false;
            }

        }

    }
}
