using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace CurrencyAssistent.Comunicators
{
    public static class NewVersionDownloader
    {
        public const string GIT_REPO = "https://api.github.com/repos/BroukalJa/CurrencyAssistant/releases/latest";
        public static void GetNewVersion()
        {
            using (WebClient wc = new WebClient())
            {
                //wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                //wc.
                var data = Json.Decode(wc.DownloadString(GIT_REPO));
                //RepCollections rep = JsonConvert.DeserializeObject<RepCollections>(rawJSON);
                ;
                //wc.DownloadFileAsync(new System.Uri(path), GetFileFolder + file);
                //wc.BaseAddress = GIT_REPO;
            }
        }
    }
}
