using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAssistent.Exports
{
    public static class ExcelExport
    {
        public static byte[] ExportCurrencies()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                using (var workbook = SpreadsheetDocument.Create(mem, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();

                    workbook.WorkbookPart.Workbook = new Workbook();

                    workbook.WorkbookPart.AddNewPart<WorkbookStylesPart>().Stylesheet = createStylesheetCurrencies();

                    workbook.WorkbookPart.Workbook.Sheets = new Sheets();

                    var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();

                    SpreadsheetPrinterSettingsPart spreadsheetPrinterSettingsPart1 = sheetPart.AddNewPart<SpreadsheetPrinterSettingsPart>();

                    sheetPart.Worksheet = new Worksheet();

                    SheetProperties sp = new SheetProperties(new PageSetupProperties());
                    sheetPart.Worksheet.Append(sp);

                    sheetPart.Worksheet.SheetProperties.PageSetupProperties.FitToPage = BooleanValue.FromBoolean(true);

                    Columns cols = new Columns();
                    UInt32Value ind = 1U;
                    cols.AppendChild(new Column() { Width = 15, CustomWidth = true, Min = ind, Max = ind });
                    ind++;
                    cols.AppendChild(new Column() { Width = 15, CustomWidth = true, Min = ind, Max = ind });
                    ind++;
                    cols.AppendChild(new Column() { Width = 30, CustomWidth = true, Min = ind, Max = ind });
                    ind++;
                    cols.AppendChild(new Column() { Width = 20, CustomWidth = true, Min = ind, Max = ind });
                    ind++;
                    cols.AppendChild(new Column() { Width = 20, CustomWidth = true, Min = ind, Max = ind });
                    ind++;
                    cols.AppendChild(new Column() { Width = 20, CustomWidth = true, Min = ind, Max = ind });
                    ind++;
                    cols.AppendChild(new Column() { Width = 20, CustomWidth = true, Min = ind, Max = ind });
                    ind++;
                    cols.AppendChild(new Column() { Width = 20, CustomWidth = true, Min = ind, Max = ind });
                    ind++;
                    cols.AppendChild(new Column() { Width = 20, CustomWidth = true, Min = ind, Max = ind });
                    ind++;
                    cols.AppendChild(new Column() { Width = 20, CustomWidth = true, Min = ind, Max = ind });
                    ind++;
                    cols.AppendChild(new Column() { Width = 20, CustomWidth = true, Min = ind, Max = ind });

                    sheetPart.Worksheet.Append(cols);
                    var sheetData = new SheetData();
                    sheetPart.Worksheet.Append(sheetData);
                    MergeCells mergeCells1 = new MergeCells();
                    mergeCells1.AppendChild(new MergeCell() { Reference = "A1:A2" });
                    mergeCells1.AppendChild(new MergeCell() { Reference = "B1:B2" });
                    mergeCells1.AppendChild(new MergeCell() { Reference = "D1:E1" });
                    mergeCells1.AppendChild(new MergeCell() { Reference = "F1:G1" });
                    mergeCells1.AppendChild(new MergeCell() { Reference = "H1:I1" });
                    mergeCells1.AppendChild(new MergeCell() { Reference = "J1:K1" });

                    sheetPart.Worksheet.Append(mergeCells1);

                    Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                    string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                    uint sheetId = 1;
                    if (sheets.Elements<Sheet>().Count() > 0)
                    {
                        sheetId =
                            sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = "Měnové kurzy" };
                    sheets.Append(sheet);

                    Row row1 = new Row() { Height = 24, CustomHeight = true };
                    row1.AppendChild(new Cell() { StyleIndex = 1, CellValue = new CellValue("DATUM"), DataType = CellValues.String });
                    row1.AppendChild(new Cell() { StyleIndex = 1, CellValue = new CellValue("POČET"), DataType = CellValues.String });
                    row1.AppendChild(new Cell() { StyleIndex = 1, CellValue = new CellValue(DataClass.Currency.GetBankName(DataClass.BankEnumerator.CNB)), DataType = CellValues.String });
                    row1.AppendChild(new Cell() { StyleIndex = 7, CellValue = new CellValue(DataClass.Currency.GetBankName(DataClass.BankEnumerator.CSOB)), DataType = CellValues.String });
                    row1.AppendChild(new Cell() { StyleIndex = 3, DataType = CellValues.String });
                    row1.AppendChild(new Cell() { StyleIndex = 7, CellValue = new CellValue(DataClass.Currency.GetBankName(DataClass.BankEnumerator.RB)), DataType = CellValues.String });
                    row1.AppendChild(new Cell() { StyleIndex = 3, DataType = CellValues.String });
                    row1.AppendChild(new Cell() { StyleIndex = 7, CellValue = new CellValue(DataClass.Currency.GetBankName(DataClass.BankEnumerator.KB)), DataType = CellValues.String });
                    row1.AppendChild(new Cell() { StyleIndex = 3, DataType = CellValues.String });
                    row1.AppendChild(new Cell() { StyleIndex = 7, CellValue = new CellValue(DataClass.Currency.GetBankName(DataClass.BankEnumerator.SPORITELNA)), DataType = CellValues.String });
                    row1.AppendChild(new Cell() { StyleIndex = 3, DataType = CellValues.String });
                    sheetData.AppendChild(row1);

                    Row row2 = new Row() { Height = 24, CustomHeight = true };
                    row2.AppendChild(new Cell() { StyleIndex = 1, DataType = CellValues.String });
                    row2.AppendChild(new Cell() { StyleIndex = 1, DataType = CellValues.String });
                    row2.AppendChild(new Cell() { StyleIndex = 1, CellValue = new CellValue("prodej"), DataType = CellValues.String });
                    row2.AppendChild(new Cell() { StyleIndex = 7, CellValue = new CellValue("prodej"), DataType = CellValues.String });
                    row2.AppendChild(new Cell() { StyleIndex = 3, CellValue = new CellValue("nákup"), DataType = CellValues.String });
                    row2.AppendChild(new Cell() { StyleIndex = 7, CellValue = new CellValue("prodej"), DataType = CellValues.String });
                    row2.AppendChild(new Cell() { StyleIndex = 3, CellValue = new CellValue("nákup"), DataType = CellValues.String });
                    row2.AppendChild(new Cell() { StyleIndex = 7, CellValue = new CellValue("prodej"), DataType = CellValues.String });
                    row2.AppendChild(new Cell() { StyleIndex = 3, CellValue = new CellValue("nákup"), DataType = CellValues.String });
                    row2.AppendChild(new Cell() { StyleIndex = 7, CellValue = new CellValue("prodej"), DataType = CellValues.String });
                    row2.AppendChild(new Cell() { StyleIndex = 3, CellValue = new CellValue("nákup"), DataType = CellValues.String });
                    sheetData.AppendChild(row2);

                    int rowIndex = 3;

                    foreach (var cur in CurrencySingleton.Instance.Currencies)
                    {
                        Row currencyHeaderRow = new Row() { Height = 24, CustomHeight = true };
                        currencyHeaderRow.AppendChild(new Cell() { StyleIndex = 2, CellValue = new CellValue(cur.Name), DataType = CellValues.String });
                        currencyHeaderRow.AppendChild(new Cell() { StyleIndex = 2 });
                        currencyHeaderRow.AppendChild(new Cell() { StyleIndex = 2 });
                        currencyHeaderRow.AppendChild(new Cell() { StyleIndex = 2 });
                        currencyHeaderRow.AppendChild(new Cell() { StyleIndex = 2 });
                        currencyHeaderRow.AppendChild(new Cell() { StyleIndex = 2 });
                        currencyHeaderRow.AppendChild(new Cell() { StyleIndex = 2 });
                        currencyHeaderRow.AppendChild(new Cell() { StyleIndex = 2 });
                        currencyHeaderRow.AppendChild(new Cell() { StyleIndex = 2 });
                        currencyHeaderRow.AppendChild(new Cell() { StyleIndex = 2 });
                        currencyHeaderRow.AppendChild(new Cell() { StyleIndex = 2 });
                        sheetData.AppendChild(currencyHeaderRow);
                        mergeCells1.AppendChild(new MergeCell() { Reference = "A" + rowIndex + ":K" + rowIndex + "" });
                        rowIndex++;
                        var dates = cur.BankRates[cur.BankRates.Keys.First()].Keys.ToList();
                        foreach (var date in dates)
                        {
                            Row newRow = new Row() { Height = 24, CustomHeight = true };
                            newRow.AppendChild(new Cell() { StyleIndex = 1, CellValue = new CellValue(date.ToShortDateString()), DataType = CellValues.Date });
                            newRow.AppendChild(new Cell() { StyleIndex = 1, CellValue = new CellValue(cur.BankRates[cur.BankRates.Keys.First()][date].Amount.ToString()), DataType = CellValues.Number });
                            if (cur.BankRates.Keys.Contains(DataClass.BankEnumerator.CNB))
                                newRow.AppendChild(new Cell() { StyleIndex = 5, CellValue = new CellValue(cur.BankRates[DataClass.BankEnumerator.CNB][date].SellRate.ToString().Replace(',','.')), DataType = CellValues.Number });
                            else
                                newRow.AppendChild(new Cell() { StyleIndex = 5 });
                            if (cur.BankRates.ContainsKey(DataClass.BankEnumerator.CSOB) && cur.BankRates[DataClass.BankEnumerator.CSOB].ContainsKey(date))
                            {
                                newRow.AppendChild(new Cell() { StyleIndex = 6, CellValue = new CellValue(cur.BankRates[DataClass.BankEnumerator.CSOB][date].SellRate.ToString().Replace(',', '.')), DataType = CellValues.Number });
                                newRow.AppendChild(new Cell() { StyleIndex = 5, CellValue = new CellValue(cur.BankRates[DataClass.BankEnumerator.CSOB][date].BuyRate.ToString().Replace(',', '.')), DataType = CellValues.Number });
                            }
                            else
                            {
                                newRow.AppendChild(new Cell() { StyleIndex = 6 });
                                newRow.AppendChild(new Cell() { StyleIndex = 5 });
                            }
                            if (cur.BankRates.ContainsKey(DataClass.BankEnumerator.RB) && cur.BankRates[DataClass.BankEnumerator.RB].ContainsKey(date))
                            {
                                newRow.AppendChild(new Cell() { StyleIndex = 6, CellValue = new CellValue(cur.BankRates[DataClass.BankEnumerator.RB][date].SellRate.ToString().Replace(',', '.')), DataType = CellValues.Number });
                                newRow.AppendChild(new Cell() { StyleIndex = 5, CellValue = new CellValue(cur.BankRates[DataClass.BankEnumerator.RB][date].BuyRate.ToString().Replace(',', '.')), DataType = CellValues.Number });
                            }
                            else
                            {
                                newRow.AppendChild(new Cell() { StyleIndex = 6 });
                                newRow.AppendChild(new Cell() { StyleIndex = 5 });
                            }
                            if (cur.BankRates.ContainsKey(DataClass.BankEnumerator.KB) && cur.BankRates[DataClass.BankEnumerator.KB].ContainsKey(date))
                            {
                                newRow.AppendChild(new Cell() { StyleIndex = 6, CellValue = new CellValue(cur.BankRates[DataClass.BankEnumerator.KB][date].SellRate.ToString().Replace(',', '.')), DataType = CellValues.Number });
                                newRow.AppendChild(new Cell() { StyleIndex = 5, CellValue = new CellValue(cur.BankRates[DataClass.BankEnumerator.KB][date].BuyRate.ToString().Replace(',', '.')), DataType = CellValues.Number });
                            }
                            else
                            {
                                newRow.AppendChild(new Cell() { StyleIndex = 6 });
                                newRow.AppendChild(new Cell() { StyleIndex = 5 });
                            }
                            if (cur.BankRates.ContainsKey(DataClass.BankEnumerator.SPORITELNA) && cur.BankRates[DataClass.BankEnumerator.SPORITELNA].ContainsKey(date))
                            {
                                newRow.AppendChild(new Cell() { StyleIndex = 6, CellValue = new CellValue(cur.BankRates[DataClass.BankEnumerator.SPORITELNA][date].SellRate.ToString().Replace(',', '.')), DataType = CellValues.Number });
                                newRow.AppendChild(new Cell() { StyleIndex = 5, CellValue = new CellValue(cur.BankRates[DataClass.BankEnumerator.SPORITELNA][date].BuyRate.ToString().Replace(',', '.')), DataType = CellValues.Number });
                            }
                            else
                            {
                                newRow.AppendChild(new Cell() { StyleIndex = 6 });
                                newRow.AppendChild(new Cell() { StyleIndex = 5 });
                            }
                            sheetData.AppendChild(newRow);
                            rowIndex++;
                        }
                    }
                    
                    workbook.WorkbookPart.Workbook.Save();
                    workbook.Close();
                }
                return mem.ToArray();
            }
        }



        private static Stylesheet createStylesheetCurrencies()
        {


            Stylesheet workbookstylesheet = new Stylesheet();

            UInt32Value ID1 = 164U;
            UInt32Value ID2 = 166U;

            //<NumberingFormats>
            NumberingFormats numberingFormats1 = new NumberingFormats();
            NumberingFormat numberingFormat1 = new NumberingFormat() { NumberFormatId = ID1, FormatCode = "#,##0.0000\\ \"Kč\"" };
            NumberingFormat numberingFormat2 = new NumberingFormat() { NumberFormatId = ID2, FormatCode = "#,##0.000" };

            numberingFormats1.Append(numberingFormat1);
            numberingFormats1.Append(numberingFormat2);

            //<Fonts>
            Fonts fonts1 = new Fonts();

            Font font1 = new Font();
            FontSize fontSize1 = new FontSize() { Val = 11D };
            FontName fontName1 = new FontName() { Val = "Calibri" };

            font1.Append(fontSize1);
            font1.Append(fontName1);

            Font font2 = new Font();
            FontSize fontSize2 = new FontSize() { Val = 10D };
            FontName fontName2 = new FontName() { Val = "Lucida Sans Unicode" };

            font2.Append(fontSize2);
            font2.Append(fontName2);

          
            fonts1.Append(font1);
            fonts1.Append(font2);

            // <Fills>
            Fills fills1 = new Fills();

            Fill fill1 = new Fill();
            PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };

            fill1.Append(patternFill1);

            Fill fill2 = new Fill();
            PatternFill patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };

            fill2.Append(patternFill2);

            Fill fill3 = new Fill();

            PatternFill patternFill3 = new PatternFill() { PatternType = PatternValues.Solid };
            ForegroundColor foregroundColor1 = new ForegroundColor() { Rgb = "FFA9A9A9" };//nadpis
            BackgroundColor backgroundColor1 = new BackgroundColor() { Indexed = (UInt32Value)66U };

            patternFill3.Append(foregroundColor1);
            patternFill3.Append(backgroundColor1);

            fill3.Append(patternFill3);
            
            fills1.Append(fill1);
            fills1.Append(fill2);
            fills1.Append(fill3);

            // <Borders>
            Borders borders1 = new Borders();

            Border border0 = new Border();
            border0.TopBorder = new TopBorder();
            border0.BottomBorder = new BottomBorder();
            border0.LeftBorder = new LeftBorder();
            border0.RightBorder = new RightBorder();

            border0.TopBorder.Style = BorderStyleValues.Medium;
            border0.BottomBorder.Style = BorderStyleValues.Medium;
            border0.LeftBorder.Style = BorderStyleValues.Medium;
            border0.RightBorder.Style = BorderStyleValues.Medium;

            Border border1 = new Border();
            border1.RightBorder = new RightBorder();
            border1.LeftBorder = new LeftBorder();
            
            border1.RightBorder.Style = BorderStyleValues.Thin;
            border1.LeftBorder.Style = BorderStyleValues.Thin;

            Border border2 = new Border();
            border2.TopBorder = new TopBorder();
            border2.BottomBorder = new BottomBorder();

            border2.TopBorder.Style = BorderStyleValues.Medium;
            border2.BottomBorder.Style = BorderStyleValues.Medium;

            Border border3 = new Border();
            border3.RightBorder = new RightBorder();
            
            border3.RightBorder.Style = BorderStyleValues.Thin;
            

            // <APENDING Borders>
            borders1.Append(border0);
            borders1.Append(border1);
            borders1.Append(border2);
            borders1.Append(border3);


            //Formats
            // <CellFormats>
            CellFormats cellformats1 = new CellFormats();

            //Formát prázdno
            CellFormat cellformat0 = new CellFormat() { FontId = 0, FillId = 2, ApplyFont = true, ApplyFill = true };
            //Formát typu 2
            CellFormat cellformat1 = new CellFormat() { FontId = 1, BorderId = 1, ApplyFont = true, ApplyBorder = true };
            //Formát typu 3 pro plain text
            CellFormat cellformat2 = new CellFormat() { FontId = 1, BorderId = 2, ApplyFont = true, ApplyBorder = true };
            //Formát typu 3 pro množství
            CellFormat cellformat3 = new CellFormat() { FontId = 1, BorderId = 3, ApplyFont = true, ApplyBorder = true };
            //Formát typu 3 pro měnu
            CellFormat cellformat4 = new CellFormat() { FontId = 1, BorderId = 1, NumberFormatId = ID1, ApplyFont = true, ApplyBorder = true, ApplyNumberFormat = true };
            //Formát typu 3 pro MJ
            CellFormat cellformat5 = new CellFormat() { FontId = 1, BorderId = 3, NumberFormatId = ID1, ApplyFont = true, ApplyBorder = true, ApplyNumberFormat = true };
            CellFormat cellformat6 = new CellFormat() { FontId = 1, BorderId = 0, NumberFormatId = ID1, ApplyFont = true, ApplyBorder = true, ApplyNumberFormat = true };
            CellFormat cellformat7 = new CellFormat() { FontId = 1, BorderId = 0, ApplyFont = true, ApplyBorder = true };

            // <APENDING CellFormats>
            cellformats1.Append(cellformat0);
            cellformats1.Append(cellformat1);
            cellformats1.Append(cellformat2);
            cellformats1.Append(cellformat3);
            cellformats1.Append(cellformat4);
            cellformats1.Append(cellformat5);
            cellformats1.Append(cellformat6);
            cellformats1.Append(cellformat7);


            // Append NUMBERINGFORMATS, FONTS, FILLS , BORDERS & CellFormats to stylesheet <Preserve the ORDER>
            workbookstylesheet.Append(numberingFormats1);
            workbookstylesheet.Append(fonts1);
            workbookstylesheet.Append(fills1);
            workbookstylesheet.Append(borders1);
            workbookstylesheet.Append(cellformats1);

            /*stylesheet.Stylesheet = workbookstylesheet;
            stylesheet.Stylesheet.Save();*/

            return workbookstylesheet;
        }
    }
}

