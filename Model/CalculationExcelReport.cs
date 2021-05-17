using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Common;
using Model.DBStructure;
using Excel = Microsoft.Office.Interop.Excel;

namespace Model
{
    public class CalculationExcelReport
    {
        private string companyName;
        private string director;
        private string accountman;
        private Reports database;
        private string partner;
        private StreamReader streamToPrint;
        private List<Production> productions;
        private DateTime date;
        public CalculationExcelReport(DateTime date)
        {
            new InfoPageModel().Read(out companyName, out string _, out string _, out string _, out DateTime _,
                out string _, out string _, out director, out accountman, out _);
            database = new Reports();
            this.date = date;
            productions = new Reports().GetProductions(date);
            
        }
        public void CreateReport()
        {
             
            var ex = new Excel.Application {Visible = false, SheetsInNewWorkbook = 1};
            Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
            var sheet = (Excel.Worksheet)ex.Worksheets.Item[1];
            sheet.Range["a:a"].ColumnWidth = Helpers.FromPixels(40f);
            sheet.Range["b:b"].ColumnWidth = Helpers.FromPixels(610f);
            sheet.Range["c:c"].ColumnWidth = Helpers.FromPixels(75f);
            sheet.Range["d:d"].ColumnWidth = Helpers.FromPixels(75f);
            sheet.Range["E:E"].ColumnWidth = Helpers.FromPixels(75f);
            sheet.Range["f:f"].ColumnWidth = Helpers.FromPixels(75f);
            ex.DisplayAlerts = false;

        

            int start;
            
            sheet.Range["A1", "IV65536"].WrapText = true;
            int offset = 1;
            for (int i = 0; i < productions.Count; i++)
            {
                start = offset;
                Production production = productions[i];
                FillCalculation(production, ref offset, sheet);
                
                var oRange = sheet.Range[$"a{start + 4}", $"f{offset - 1}"];
                oRange.Borders.Item[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRange.Borders.Item[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRange.Borders.Item[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRange.Borders.Item[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRange.BorderAround();
            }

            workBook.SaveAs($"{Directory.GetCurrentDirectory()}\\Калькуляция {Helpers.Monthes[date.Month]}.xls");
            workBook.Close();
            
        }

        public void CreateConsumptionRate()
        {
            var ex = new Excel.Application {Visible = false, SheetsInNewWorkbook = 1};
            Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
            var sheet = (Excel.Worksheet)ex.Worksheets.Item[1];
            sheet.Range["a:a"].ColumnWidth = Helpers.FromPixels(50f);
            sheet.Range["b:b"].ColumnWidth = Helpers.FromPixels(640f);
            sheet.Range["c:c"].ColumnWidth = Helpers.FromPixels(75f);
            sheet.Range["d:d"].ColumnWidth = Helpers.FromPixels(75f);
            
            ex.DisplayAlerts = false;

        

            int start;
            
            sheet.Range["A1", "IV65536"].WrapText = true;
            int offset = 1;
            for (int i = 0; i < productions.Count; i++)
            {
                start = offset;
                Production production = productions[i];
                FillConsumptionRate(production, ref offset, sheet);
                
                var oRange = sheet.Range[$"a{start + 4}", $"d{offset - 1}"];
                oRange.Borders.Item[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRange.Borders.Item[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRange.Borders.Item[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRange.Borders.Item[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRange.BorderAround();

                offset += 3;
            }

            workBook.SaveAs($"{Directory.GetCurrentDirectory()}\\Нормы расхода {Helpers.Monthes[date.Month]}.xls");
            workBook.Close();
        }

        public void CreateAct()
        {
            var ex = new Excel.Application {Visible = false, SheetsInNewWorkbook = 1};
            Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
            var sheet = (Excel.Worksheet)ex.Worksheets.Item[1];
            sheet.Range["a:a"].ColumnWidth = Helpers.FromPixels(40f);
            sheet.Range["b:b"].ColumnWidth = Helpers.FromPixels(610f);
            sheet.Range["c:c"].ColumnWidth = Helpers.FromPixels(75f);
            sheet.Range["d:d"].ColumnWidth = Helpers.FromPixels(75f);
            sheet.Range["E:E"].ColumnWidth = Helpers.FromPixels(75f);
            sheet.Range["f:f"].ColumnWidth = Helpers.FromPixels(75f);
            ex.DisplayAlerts = false;

        

            int start;
            
            sheet.Range["A1", "IV65536"].WrapText = true;
            int offset = 1;
            for (int i = 0; i < productions.Count; i++)
            {
                start = offset;
                Production production = productions[i];
                FillAct(production, ref offset, sheet);
                
                var oRange = sheet.Range[$"a{start + 4}", $"f{offset - 1}"];
                oRange.Borders.Item[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRange.Borders.Item[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRange.Borders.Item[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRange.Borders.Item[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRange.BorderAround();
            }

            workBook.SaveAs($"{Directory.GetCurrentDirectory()}\\Акт {Helpers.Monthes[date.Month]}.xls");
            workBook.Close();
        }
        private void FillCalculation(Production production, ref int offset, Excel.Worksheet sheet)
        {
            partner = database.GetPartnerName(production.PartnerId);
            sheet.Cells[offset++, 2] = "Утверждаю";
            sheet.Cells[offset++, 2] = $"Директор {companyName}";
            sheet.Cells[offset++, 2] = $"{director}";
            sheet.Cells[offset++, 2] = Helpers.DateName(production.Date);

            sheet.Range[sheet.Cells[offset++, 1], sheet.Cells[offset, 6]].Merge();
            sheet.Cells[offset - 1, 1] = $"Плановая калькуляция отпускной цены на изготовление {production.Name} согласно договора {production.DocumentNumber} от {Helpers.DatePoints(production.Date)} для {partner}";
            sheet.Range[$"a{offset - 1}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            offset++;
            sheet.Cells[offset, 1] = "№ п/п";
            sheet.Cells[offset, 2] = "Наименование";
            sheet.Cells[offset, 3] = "един. изм.";
            sheet.Cells[offset, 4] = "количество";
            sheet.Cells[offset, 5] = "цена";
            sheet.Cells[offset++, 6] = "сумма";
            
            sheet.Cells[offset, 1] = "1";
            sheet.Cells[offset, 2] = "Покупные материалы";

            float sumOfMaterials = database.GetSumOfMaterials(production);
            sheet.Cells[offset++, 6] = $"{Math.Round(sumOfMaterials, 2)}";

            List<ExcelExpence> list = new Reports().GetListForExcelReport(production);
            for (int index = 0; index < list.Count; index++)
            {
                ExcelExpence excelExpence = list[index];
                sheet.Cells[offset, 1] = $"1.{index + 1}";
                sheet.Cells[offset, 2] = $"{excelExpence.Name}";
                sheet.Cells[offset, 3] = $"{excelExpence.Unity}";
                sheet.Cells[offset, 4] = $"{excelExpence.Count}";
                sheet.Cells[offset, 5] = $"{excelExpence.Price}";
                sheet.Cells[offset++, 6] = $"{excelExpence.Cost}";
                
            }
            double salary = Math.Round(2.79f * production.HoursForProd, 2);
            double fullSalary = Math.Round(salary * 1.8, 2);
            double culletPrice = Math.Round(production.Cullet * 0.61f, 2);
            double aluminumPrice = Math.Round(production.Aluminum * 0.61, 2);
            double steelPrice = Math.Round(production.Steel * 0.61, 2);
            double fszn = Math.Round(fullSalary * 0.34, 2);
            double belGos = Math.Round(fullSalary * 0.0052, 2);
            double transportationPrice = Math.Round(production.Transportation, 2);
            double overheadPrice = Math.Round(fullSalary * production.Overhead / 100, 2);
            double cost =
                Math.Round(
                    sumOfMaterials + fullSalary + fszn + belGos + overheadPrice +
                    transportationPrice - steelPrice - aluminumPrice - culletPrice, 2);
            double price = Math.Round(production.SellingPrice, 2);
            double tax = Math.Round(price / 6, 2);
            double priceWithoutTax = Math.Round(cost - tax, 2);
            double profit = Math.Round(cost - priceWithoutTax, 2);
            double profitability = profit / cost * 100;
            
            sheet.Cells[offset, 2] = "Возвратные отходы - стеклобой";
            sheet.Cells[offset, 3] = "кг";
            sheet.Cells[offset, 4] = $"{Math.Round(production.Cullet, 2)}";
            sheet.Cells[offset, 5] = "";
            sheet.Cells[offset++, 6] = $"{culletPrice}";
            
            sheet.Cells[offset, 2] = "Возвратные отходы - лом и отходы сталь";
            sheet.Cells[offset, 3] = "кг";
            sheet.Cells[offset, 4] = $"{Math.Round(production.Steel, 2)}";
            sheet.Cells[offset, 5] = "";
            sheet.Cells[offset++, 6] = $"{steelPrice}";
            
            sheet.Cells[offset, 2] = "Возвратные отходы - лом и отходы алюминия";
            sheet.Cells[offset, 3] = "кг";
            sheet.Cells[offset, 4] = $"{production.Aluminum}";
            sheet.Cells[offset, 5] = "0.61";
            sheet.Cells[offset++, 6] = $"{aluminumPrice}";
            
            sheet.Cells[offset, 1] = "2";
            sheet.Cells[offset, 2] = "Основная и дополнительная заработная плата";
            sheet.Cells[offset++, 6] = $"{fullSalary}";
            
            sheet.Cells[offset, 1] = "3";
            sheet.Cells[offset, 2] = "Начисления на заработную плату, в том числе:";
            sheet.Cells[offset++, 6] = $"{fszn + belGos}";
            
            sheet.Cells[offset, 1] = "3.1";
            sheet.Cells[offset, 2] = "Отчисления в ФСЗН 34%";
            sheet.Cells[offset++, 6] = $"{fszn}";
            
            sheet.Cells[offset, 1] = "3.2";
            sheet.Cells[offset, 2] = "Отчисления в Белгосстрах 0,52%";
            sheet.Cells[offset++, 6] = $"{belGos}";
            
            sheet.Cells[offset, 1] = "4";
            sheet.Cells[offset, 2] = $"Накладные расходы {production.Overhead}%";
            sheet.Cells[offset++, 6] = $"{overheadPrice}";
            
            sheet.Cells[offset, 1] = "5";
            sheet.Cells[offset, 2] = "Транспортно-заготовительные расходы";
            sheet.Cells[offset++, 6] = Math.Round(production.Transportation, 2);

            
            sheet.Cells[offset, 1] = "6";
            sheet.Cells[offset, 2] = "Себестоимость";
            sheet.Cells[offset++, 6] = $"{cost}";
            
            sheet.Cells[offset, 1] = "7";
            sheet.Cells[offset, 2] = "Транспортные расходы по доставке";
            sheet.Cells[offset++, 6] = $"{transportationPrice}";
            
            sheet.Cells[offset, 1] = "8";
            sheet.Cells[offset, 2] = "Полная себестоимость";
            sheet.Cells[offset++, 6] = $"{Math.Round(cost + transportationPrice, 2)}";
            
            sheet.Cells[offset, 1] = "9";
            sheet.Cells[offset, 2] = "Рентабельность";
            sheet.Cells[offset++, 6] = $"{profitability}";
            
            sheet.Cells[offset, 1] = "10";
            sheet.Cells[offset, 2] = "Прибыль";
            sheet.Cells[offset++, 6] = $"{profit.ToString(CultureInfo.CurrentCulture)}";
            
            sheet.Cells[offset, 1] = "11";
            sheet.Cells[offset, 2] = "Стоимость изделия без НДС";
            sheet.Cells[offset++, 6] = $"{priceWithoutTax}";
            
            sheet.Cells[offset, 1] = "12";
            sheet.Cells[offset, 2] = "НДС 20%";
            sheet.Cells[offset++, 6] = $"{tax}";
            
            sheet.Cells[offset, 1] = "13";
            sheet.Cells[offset, 2] = "Отпускная цена";
            sheet.Cells[offset++, 6] = $"{cost}";

            sheet.Cells[offset, 2] = "Главный бухгалтер";
            sheet.Cells[offset++, 5] = accountman;
            sheet.Range[sheet.Cells[offset++, 1], sheet.Cells[offset, 6]].Merge();
            sheet.Cells[offset - 1, 1] = $"Расчёт заработной платы на изготовление {production.Name} " +
                                         $"согласно договора № {production.DocumentNumber} от {Helpers.DatePoints(production.Date)} " +
                                         $"для {partner}";
            sheet.Range[$"a{offset - 1}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            
            offset++;
            sheet.Range[sheet.Cells[offset, 1], sheet.Cells[offset, 2]].Merge();
            sheet.Cells[offset, 1] = "Наименование статей";
            sheet.Range[sheet.Cells[offset, 3], sheet.Cells[offset, 4]].Merge();
            sheet.Cells[offset, 3] = "кол-во часов на изготовление";
            sheet.Cells[offset, 5] = "Зарплата за 1ч";
            sheet.Cells[offset++, 6] = "Сумма (руб)";

            sheet.Range[sheet.Cells[offset, 1], sheet.Cells[offset, 2]].Merge();
            sheet.Range[sheet.Cells[offset, 3], sheet.Cells[offset, 4]].Merge();
            sheet.Cells[offset, 1] = "Повременная зарплата слесаря по сборке металлоконструкций";
            sheet.Cells[offset, 3] = $"{production.HoursForProd}";
            sheet.Cells[offset, 5] = "2.79";
            
            sheet.Cells[offset++, 6] = $"{salary}";
            sheet.Range[sheet.Cells[offset, 1], sheet.Cells[offset, 2]].Merge();
            sheet.Range[sheet.Cells[offset, 3], sheet.Cells[offset, 4]].Merge();
            sheet.Cells[offset, 1] = "Премиальные 80%";
            sheet.Cells[offset++, 6] = $"{salary * 0.8}";
            sheet.Range[sheet.Cells[offset, 1], sheet.Cells[offset, 2]].Merge();
            sheet.Range[sheet.Cells[offset, 3], sheet.Cells[offset, 4]].Merge();
            sheet.Cells[offset, 1] = "ИТОГО";
            sheet.Cells[offset++, 6] = $"{fullSalary}";

        }

        private void FillConsumptionRate(Production production, ref int offset, Excel.Worksheet sheet)
        {
            partner = database.GetPartnerName(production.PartnerId);
            sheet.Cells[offset++, 2] = "Утверждаю";
            sheet.Cells[offset++, 2] = $"Директор {companyName}";
            sheet.Cells[offset++, 2] = $"{director}";
            sheet.Cells[offset++, 2] = Helpers.DateName(production.Date);

            sheet.Range[sheet.Cells[offset++, 1], sheet.Cells[offset, 6]].Merge();
            sheet.Cells[offset - 1, 1] = $"Нормы расхода материалов на изготовлние {production.Name} согласно договора {production.DocumentNumber} от {Helpers.DatePoints(production.Date)} для {partner}";
            sheet.Range[$"a{offset - 1}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            offset++;
            
            sheet.Cells[offset, 1] = "№ п/п";
            sheet.Cells[offset, 2] = "Наименование";
            sheet.Cells[offset, 3] = "един. изм.";
            sheet.Cells[offset, 4] = "количество";
            
            List<ExcelExpence> list = new Reports().GetListForExcelReport(production);
            for (int index = 0; index < list.Count; index++)
            {
                ExcelExpence excelExpence = list[index];
                if (excelExpence.Count - excelExpence.InstallCount > 0)
                {
                    sheet.Cells[offset, 1] = $"1.{index + 1}";
                    sheet.Cells[offset, 2] = $"{excelExpence.Name}";
                    sheet.Cells[offset, 3] = $"{excelExpence.Unity}";
                    sheet.Cells[offset, 4] = $"{excelExpence.Count - excelExpence.InstallCount}";
                }
                
            }
            
            sheet.Range[sheet.Cells[offset++, 1], sheet.Cells[offset, 6]].Merge();
            sheet.Cells[offset - 1, 1] = $"Нормы расхода материалов на комплект для установки изделия {production.Name} согласно договора {production.DocumentNumber} от {Helpers.DatePoints(production.Date)} для {partner}";
            sheet.Range[$"a{offset - 1}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            offset++;
            
            for (int index = 0; index < list.Count; index++)
            {
                ExcelExpence excelExpence = list[index];
                if (excelExpence.InstallCount > 0)
                {
                    sheet.Cells[offset, 1] = $"1.{index + 1}";
                    sheet.Cells[offset, 2] = $"{excelExpence.Name}";
                    sheet.Cells[offset, 3] = $"{excelExpence.Unity}";
                    sheet.Cells[offset, 4] = $"{excelExpence.InstallCount}";
                }
                
            }

            sheet.Cells[offset, 2] = "Главный инженер";
            sheet.Cells[offset++, 3] = "";

        }

        private void FillAct(Production production, ref int offset, Excel.Worksheet sheet)
        {
            partner = database.GetPartnerName(production.PartnerId);
            sheet.Cells[offset++, 2] = $"{companyName}";
            sheet.Cells[offset++, 2] = $"Акт № ";
            sheet.Cells[offset++, 2] = $" от {Helpers.DateName(new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)))}";
            
            sheet.Cells[offset++, 2] = "о списании товарно-материальных ценностей";
            sheet.Cells[offset++, 2] = "Основание: ";
            sheet.Cells[offset++, 2] = "составлен комиссией";
            offset += 3;
            sheet.Cells[offset++, 2] = "Материально ответственное лицо: ";
            sheet.Cells[offset++, 2] =
                "Комиссия составила настоящий акт в том, что указанные ниже товарно-материальные ценности";
            
            sheet.Cells[offset++, 2] = "были использованы за период с 1 по 31 марта 2021г на изготовление следующей продукции: ";
            
            sheet.Range[sheet.Cells[offset++, 1], sheet.Cells[offset, 6]].Merge();
            sheet.Cells[offset - 1, 1] = $" {production.Name} согласно договора {production.DocumentNumber} от {Helpers.DatePoints(production.Date)} для {partner}";
            sheet.Range[$"a{offset - 1}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            offset++;
            sheet.Cells[offset, 1] = "№ п/п";
            sheet.Cells[offset, 2] = "Наименование";
            sheet.Cells[offset, 3] = "един. изм.";
            sheet.Cells[offset, 4] = "количество";
            sheet.Cells[offset, 5] = "цена";
            sheet.Cells[offset++, 6] = "сумма";
            
            float sumOfMaterials = database.GetSumOfMaterials(production);
            

            List<ExcelExpence> list = new Reports().GetListForExcelReport(production);
            for (int index = 0; index < list.Count; index++)
            {
                ExcelExpence excelExpence = list[index];
                sheet.Cells[offset, 1] = $"1.{index + 1}";
                sheet.Cells[offset, 2] = $"{excelExpence.Name}";
                sheet.Cells[offset, 3] = $"{excelExpence.Unity}";
                sheet.Cells[offset, 4] = $"{excelExpence.Count}";
                sheet.Cells[offset, 5] = $"{excelExpence.Price}";
                sheet.Cells[offset++, 6] = $"{excelExpence.Cost}";
            }

            sheet.Range[sheet.Cells[offset, 1], sheet.Cells[offset, 5]].Merge();
            sheet.Cells[offset, 1] = "ИТОГО";
            sheet.Cells[offset, 6] = sumOfMaterials;
        }
    }
}