using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if SILVERLIGHT
using System.Runtime.InteropServices.Automation;
#else
#endif

namespace Utils_CS.Excel
{
    public static class Exporter
    {
        static void Main(string[] args)
        {
            // Código para iniciar tu aplicación
        }

        public static object CurrentExcel { get; set; }
        public static void ExportExcel(object[,] data)
        {
            ExportExcel(data, true);
        }

        public static void ExportExcel(object[,] data, bool isExcel)
        {
            ExportExcel(new List<object[,]> { data }, isExcel);
        }

        public static void ExportExcel(List<object> data)
        {
            ExportExcel(data);
        }

        public static void ExportExcel(List<object[,]> data, bool isExcel)
        {

#if SILVERLIGHT
            dynamic excel = AutomationFactory.CreateObject("Excel.Application");
#else
            dynamic excel = Microsoft.VisualBasic.Interaction.CreateObject("Excel.Application", string.Empty);
#endif
            excel.ScreenUpdating = false;
            dynamic workbook = excel.workbooks;
            workbook.add();

            foreach(var d in data)
            {
                dynamic worksheet = excel.Worksheets.Add();

                const int left = 1;
                const int top = 1;
                int height = d.GetLength(0);
                int width = d.GetLength(1);
                int bottom = top + height - 1;
                int right = left + width - 1;
                if (height == 0 || width == 0) return;

                dynamic rg = worksheet.Range[worksheet.Cells[top, left], worksheet.Cells[bottom, right]];
#if SILVERLIGHT
                if (isExcel)
                    rg.NumberFormat = "@";

                for (int i = 1; i <= width; i++)
                {
                    objet[] column = new object[height];
                    for (int j = 1; j <= height; j++)
                    {
                        column[j-1] = d[j - 1, i - 1];
                    }
                    dynamic r = worksheet.Range[worksheet.Cells[top, i], worksheet.Cells[bottom, i]];
                    r.Value = column;
                    r = null;
                }
#else
                if (isExcel)
                {
                    rg.NumberFormat = "@";
                }
                try
                {
                    rg.Value = d;
                } catch (Exception ex)
                {
                    string s = ex.Message;
                }
#endif
                for (int i = 1; i <= 4;i++) {
                    rg.Borders[i].LineStyle = 1;
                }

                rg.EntireColumn.AutoFit();
                dynamic rgHeader = worksheet.Range[worksheet.Cells[top, left], worksheet.cells[top, right]];
                rgHeader.Font.Bold = true;
                rgHeader.Interior.Color = 189 * (int)Math.Pow(16, 4) + 129 * (int)Math.Pow(16, 2) + 78;
                excel.ScreenUpdating = true;
                excel.Visible = true;

                CurrentExcel = excel;

                rg = null;
                rgHeader = null;
                worksheet = null;

            }

            workbook = null;
            excel = null;
            GC.Collect();
        }
    }
}
