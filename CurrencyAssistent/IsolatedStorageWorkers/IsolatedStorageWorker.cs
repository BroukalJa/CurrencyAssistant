using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAssistent.IsolatedStorageWorkers
{
    public static class IsolatedStorageWorker
    {
        public static void SerializeObjectIS()
        {

            using (var store = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, typeof(System.Security.Policy.Url), typeof(System.Security.Policy.Url)))
            {
                var path = System.IO.Path.Combine("currencyassistant", "currencyassistant.txt");

                if (!store.DirectoryExists("currencyassistant"))
                {
                    store.CreateDirectory("currencyassistant");
                }
                //xsSubmit.Serialize(writer, pram);
                //xml = ; // Your XML
                using (var stream = store.OpenFile(path, System.IO.FileMode.Create))
                {
                    //byte[] info = new UTF8Encoding(true).GetBytes(sww.ToString());
                    //stream.Write(info, 0, info.Length);
                    IFormatter fom = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    ISStore app = new ISStore();
                    foreach (var cur in CurrencySingleton.Instance.Currencies)
                    {
                        if (cur.Visible)
                            app.CheckedCurrencies.Add(cur.Name);
                    }
                    fom.Serialize(stream, app);
                }

                //
            }
        }

        public static Task<T> DeserializeObjectAsync<T>(string path)
        {
            return Task.Run(() =>
            {
                using (var store = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, typeof(System.Security.Policy.Url), typeof(System.Security.Policy.Url)))
                {
                    using (IsolatedStorageFileStream isfs = new IsolatedStorageFileStream((path), System.IO.FileMode.Open, store))
                    {
                        //isfs.reada
                        var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        return (T)binaryFormatter.Deserialize(isfs);
                    }
                }
            });
        }

        public static void LoadIS()
        {
            using (var store = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, typeof(System.Security.Policy.Url), typeof(System.Security.Policy.Url)))
            {
                var path = System.IO.Path.Combine("currencyassistant", "currencyassistant.txt");
                if (store.DirectoryExists("currencyassistant") && store.FileExists(path))
                {
                    DeserializeObjectAsync<ISStore>(path).ContinueWith(task =>
                    {
                        if (task.Result != null && task.Exception == null)
                            CurrencySingleton.Instance.ISStore = task.Result;
                    });
                }
                else
                    CurrencySingleton.Instance.ISStore = new ISStore();
               
            }
        }

        public static void UlozIS()
        {
            SerializeObjectIS();
        }
        [Serializable]
        public class ISStore
        {
            public List<string> CheckedCurrencies { get; set; } = new List<string>();
        }
    }
}
