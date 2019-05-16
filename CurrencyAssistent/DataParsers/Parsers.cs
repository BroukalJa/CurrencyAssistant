using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace CurrencyAssistent.DataParsers
{
    public static class Parsers
    {
        public static void CurrencyParser()
        {
            if (!File.Exists(Comunicators.CurrencyDownloader.GetFileFolder + "data.csv"))
                return;
            var file = File.ReadAllLines(Comunicators.CurrencyDownloader.GetFileFolder + "data.csv", Encoding.UTF8).ToList();
            foreach (var line in file)
            {
                var splt = line.Split(';');
                if (Enum.TryParse(splt[1], out DataClass.BankEnumerator bank))
                {
                    if (splt[3] != String.Empty)
                        CurrencySingleton.Instance.AddCurrencyRates(splt[0], bank, Decimal.Parse(splt[2]), Decimal.Parse(splt[3]), Int32.Parse(splt[4]), DateTime.Parse(splt[5]));
                    else
                        CurrencySingleton.Instance.AddCurrencyRates(splt[0], bank, Decimal.Parse(splt[2]), null, Int32.Parse(splt[4]), DateTime.Parse(splt[5]));
                }
            }
            foreach (var cur in CurrencySingleton.Instance.Currencies)
            {
                var blist = cur.BankRates.Keys.ToList();
                foreach (var bank in blist)
                {
                    //cur.BankRates[bank] = cur.BankRates[bank].OrderBy(x => x.Key).ToDictionary(v => v.Key, v => v.Value);
                }
            }
        }

        public static void CurrencyArchiver()
        {
            if (!File.Exists(Comunicators.CurrencyDownloader.GetFileFolder + "data.csv"))
                File.Create(Comunicators.CurrencyDownloader.GetFileFolder + "data.csv").Close();
            try
            {
                File.WriteAllText(Comunicators.CurrencyDownloader.GetFileFolder + "data.csv", CurrencySingleton.Instance.SerializeCurrencies());
            }
            catch (Exception e)
            {

            }
        }

        public static void DeleteSourceFiles()
        {
            if (File.Exists(Comunicators.CurrencyDownloader.GetFileFolder + "1.txt"))
                File.Delete(Comunicators.CurrencyDownloader.GetFileFolder + "1.txt");
            if (File.Exists(Comunicators.CurrencyDownloader.GetFileFolder + "2.txt"))
                File.Delete(Comunicators.CurrencyDownloader.GetFileFolder + "2.txt");
            if (File.Exists(Comunicators.CurrencyDownloader.GetFileFolder + "3.json"))
                File.Delete(Comunicators.CurrencyDownloader.GetFileFolder + "3.json");
            if (File.Exists(Comunicators.CurrencyDownloader.GetFileFolder + "4.txt"))
                File.Delete(Comunicators.CurrencyDownloader.GetFileFolder + "4.txt");
            if (File.Exists(Comunicators.CurrencyDownloader.GetFileFolder + "5.txt"))
                File.Delete(Comunicators.CurrencyDownloader.GetFileFolder + "5.txt");
        }

        public static void KBParser()
        {
            var file = Json.Decode(File.ReadAllText(Comunicators.CurrencyDownloader.GetFileFolder + "3.json", Encoding.UTF8))[0]["exchangeRates"];
            foreach (var cur in file)
            {
                var date = DateTime.Parse((cur["ratesValidityDate"] as string).Split('T')[0]);
                CurrencySingleton.Instance.AddCurrencyRates(cur["currencyISO"] as string, DataClass.BankEnumerator.KB, (cur["noncashSell"] as decimal?).Value, (cur["noncashBuy"] as decimal?).Value, (cur["currencyUnit"] as int?).Value, date);
            }
            File.Delete(Comunicators.CurrencyDownloader.GetFileFolder + "3.json");
        }

        public static void CSOBParser()
        {
            var file = File.ReadAllLines(Comunicators.CurrencyDownloader.GetFileFolder + "1.txt", Encoding.UTF8).ToList();
            var date = DateTime.Parse(file[0]);
            file.RemoveRange(0, 4);
            foreach (var line in file)
            {
                var splt = line.Split(';');
                CurrencySingleton.Instance.AddCurrencyRates(splt[2], DataClass.BankEnumerator.CSOB, Decimal.Parse(splt[5]), Decimal.Parse(splt[4]), Int32.Parse(splt[1]), date);
            }
            File.Delete(Comunicators.CurrencyDownloader.GetFileFolder + "1.txt");
        }

        public static void CNBParser()
        {
            var file = File.ReadAllLines(Comunicators.CurrencyDownloader.GetFileFolder + "5.txt", Encoding.UTF8).ToList();
            var date = DateTime.Parse(file[0].Split('#')[0]);
            file.RemoveRange(0, 2);
            foreach (var line in file)
            {
                var splt = line.Split('|');
                CurrencySingleton.Instance.AddCurrencyRates(splt[3], DataClass.BankEnumerator.CNB, Decimal.Parse(splt[4]), null, Int32.Parse(splt[2]), date);
            }
            File.Delete(Comunicators.CurrencyDownloader.GetFileFolder + "5.txt");
        }

        public static void RBParser()
        {
            HTMLParser(DataClass.BankEnumerator.RB, "2.txt");
        }

        public static void SporitelnaParser()
        {
            HTMLParser(DataClass.BankEnumerator.SPORITELNA, "4.txt");
        }

        public static void HTMLParser(DataClass.BankEnumerator bank, string fileName)
        {
            var file = File.ReadAllText(Comunicators.CurrencyDownloader.GetFileFolder + fileName, Encoding.UTF8).Trim().Split(new string[] { @"<table border=""0"" width=""660"" cellpadding=""1"" cellspacing=""1"" class=""pd pdw"">" }, StringSplitOptions.None).ToList();
            var date = DateTime.Parse(file[0].Split(new string[] { @"<tr><td align=""center""> <a style=""color:#000;font-weight:700"" href=""/kurzy-men/kurzovni-listek/nr/" + (bank == DataClass.BankEnumerator.RB ? "raiffeisenbank" : "ceska-sporitelna") + "/D-" }, StringSplitOptions.None)[1].Split(new string[] { @"/"">" }, StringSplitOptions.None)[0]);
            file = file[1].Split(new string[] { "</table>" }, StringSplitOptions.None)[0].Split(new string[] { "</tr>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            file.RemoveRange(0, 2);
            foreach (var line in file)
            {
                if (line.Trim() == String.Empty)
                    continue;
                var rest = line.Split(new string[] { @"<span style=""width:34px; margin-left:5px"">" }, StringSplitOptions.None)[1];
                var splt = rest.Split(new string[] { @"</span>" }, StringSplitOptions.None);
                var cur = splt[0];
                splt = splt[2].Split(new string[] { @"<td align=""right"">" }, StringSplitOptions.RemoveEmptyEntries);
                var amount = Int32.Parse(splt[1].Trim().Replace("&nbsp;</td>\r\n\t<td>�</td>", ""));
                var buyRate = Decimal.Parse(splt[2].Trim().Replace("�", "").Replace("</td>", "").Replace(".", ","));
                var sellRate = Decimal.Parse(splt[3].Trim().Replace("�", "").Replace("</td>", "").Replace(".", ","));
                CurrencySingleton.Instance.AddCurrencyRates(cur, bank, sellRate, buyRate, amount, date);
            }
            File.Delete(Comunicators.CurrencyDownloader.GetFileFolder + fileName);
        }

    }
}
