using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var dic = new Dictionary<string, string>();
                var excelPath = @"C:\Users\Leon\Desktop\配置表.xlsx";
                using (var package = new ExcelPackage(new FileInfo(excelPath)))
                {
                    var accessoriesSheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == "五金配件");
                    if (accessoriesSheet != null)
                        dic["Accessories"] = accessoriesSheet.ContentToString();

                    var panelMergeSheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == "合并表");
                    if (panelMergeSheet != null)
                        dic["PanelMerge"] = panelMergeSheet.ContentToString();

                    var panelProductCategorySheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == "柜子类型");
                    if (panelProductCategorySheet != null)
                        dic["PanelProductCategory"] = panelProductCategorySheet.ContentToString();

                    var panelProductsSheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == "风格表");
                    if (panelProductsSheet != null)
                        dic["PanelProducts"] = panelProductsSheet.ContentToString();

                    var panelReplaceProductsSheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == "替换表");
                    if (panelReplaceProductsSheet != null)
                        dic["PanelReplaceProducts"] = panelReplaceProductsSheet.ContentToString();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("error:{0}", ex.Message);
            }
            Console.WriteLine("-----------------------");
            Console.ReadKey();
        }

    }

    public static class EpplusHelper
    {
        public static string ContentToString(this ExcelWorksheet sheet)
        {
            var strBuilder = new StringBuilder();
            var endColumn = sheet.Dimension.End.Column;
            var endRow = sheet.Dimension.End.Row;
            for (int r = 1; r <= endRow; r++)
            {
                for (int c = 1; c <= endColumn; c++)
                {
                    var cell = sheet.Cells[r, c];
                    strBuilder.Append(cell.Value == null ? string.Empty : cell.Value.ToString().Trim());
                    if (c != endColumn)
                        strBuilder.Append(',');
                }
                if (r != endRow)
                    strBuilder.AppendLine();
            }

            return strBuilder.ToString();
        }
    }

}
