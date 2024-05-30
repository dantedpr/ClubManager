using System;
using System.Collections.Generic;
#if !SILVERLIGHT
using System.Data;
using System.Threading.Tasks;
#endif
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.VisualBasic;

namespace Utils_CS.Excel
{


    public static class OfficeTools
    {

        public static string dateForm = "dd/MM/yyyy HH:mm:ss";

        #region Attached Property

        public static readonly DependencyProperty IsExportProperty = DependencyProperty.RegisterAttached("IsExported", typeof(bool), typeof(DataGrid), new PropertyMetadata(true));
        public static readonly DependencyProperty HeaderForExportProperty = DependencyProperty.RegisterAttached("HeaderForExport", typeof(string), typeof(DataGrid), new PropertyMetadata(null));
        public static readonly DependencyProperty PathForExportProperty = DependencyProperty.RegisterAttached("PathForExport", typeof(string), typeof(DataGrid), new PropertyMetadata(null));
        public static readonly DependencyProperty FormatForExportProperty = DependencyProperty.RegisterAttached("FormatForExport", typeof(string), typeof(DataGrid), new PropertyMetadata(null));
        #region Attached properties helpers methods

        public static void SetIsExported(DataGridColumn element, bool value)
        {
            element.SetValue(IsExportProperty, value);  
        }

        public static Boolean GetIsExported(DataGridColumn element)
        {
            return (Boolean)element.GetValue(IsExportProperty);
        }

        public static void SetPathForExport(DataGridColumn element, bool value)
        {
            element.SetValue(PathForExportProperty, value);
        }

        public static string GetPathForExport(DataGridColumn element)
        {
            return (string)element.GetValue(PathForExportProperty);
        }

        public static void SetHeaderForExport(DataGridColumn element, bool value)
        {
            element.SetValue(HeaderForExportProperty, value);
        }

        public static string GetHeaderForExport(DataGridColumn element)
        {
            return (string)element.GetValue(HeaderForExportProperty);
        }
        public static void SetFormatForExport(DataGridColumn element, bool value)
        {
            element.SetValue(FormatForExportProperty, value);
        }

        public static string GetFormatForExport(DataGridColumn element)
        {
            return (string)element.GetValue(FormatForExportProperty);
        }
        #endregion
        #endregion

        class Params
        {

            public Params(object[,] data, bool textForm)
            {
                Data = data;
                TextForm = textForm;
            }

            public object[,] Data { get; private set; }
            public bool TextForm { get; private set; }
        }

        public static void ExportExcel(this DataGrid grid)
        {
            ExportExcel(grid, true);
        }

        public static object GetExcel()
        {
            return Exporter.CurrentExcel;
        }

        public static void ReleaseExcel()
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(Exporter.CurrentExcel);
        }

#if SILVERLIGHT
#else
        public static void ExportExcel(IEnumerable<DataGrid> grid)
        {
            ExportExcel(grid, true);
        }

        public static void ExportExcel(IEnumerable<DataGrid> grid, bool isExcel)
        {
            List<object[,]> dataCol = new List<object[,]>();
            foreach (var g in grid)
            {
                dataCol.Add(PrepareData(g));
            }

            Task.Factory.StartNew(() =>
            {
                Exporter.ExportExcel(dataCol, isExcel);
            });
        }

        public static void ExportExcel(this DataGrid grid, bool isExcel)
        {
            Exporter.CurrentExcel = null;
            Thread thread = new Thread(StartExport) { IsBackground = true };
            thread.Start(new Params(PrepareData(grid), isExcel));
        }

#endif
        private static void StartExport(object data)
        {
            var exportParams = data as Params;
            Exporter.ExportExcel(exportParams.Data, exportParams.TextForm);
        }

        private  static object[,] PrepareData(DataGrid grid)
        {
            List<DataGridColumn> cols1 = grid.Columns.Where(x => (GetIsExported(x) && ((x is DataGridColumn) || (!string.IsNullOrEmpty(GetPathForExport(x))) || (!string.IsNullOrEmpty(x.SortMemberPath))))).ToList();

            List<DataGridColumn> cols;
            if (cols1.Count == 0)
            {
                cols = grid.Columns.ToList();
            }
            else
            {
                cols = cols1;
            }

            List<object> list = grid.ItemsSource.Cast<object>().ToList();
            object[,] data = new object[list.Count + 1, cols.Count];
            for (int colIndex = 0; colIndex < cols.Count; colIndex++)
            {
                DataGridColumn gridCol = cols[colIndex];
                data[0, colIndex] = GetHeader(gridCol);
                string[] path = GetPath(gridCol);
                string formatForExport = GetFormatForExport(gridCol);

                if (path != null)
                {
                    for (int rowIndex = 1; rowIndex <= list.Count;  rowIndex++)
                    {
                        object source = list[rowIndex-1];

                        try
                        {
                            data[rowIndex, colIndex] = GetValue(path, source, formatForExport);
                        }
                        catch
                        {
                            data[rowIndex, colIndex] = "";
                        }
                    }
                }
            }
            return data;
        }

        private static string GetHeader(DataGridColumn gridCol)
        {
            string headerForExport = GetHeaderForExport(gridCol);
            if (headerForExport == null && gridCol.Header != null)
            {
                int n = Strings.InStr(gridCol.Header.ToString(), ":");
                if (n > 0)
                {
                    string xx = Strings.Mid(gridCol.Header.ToString(), n + 1, 1000);
                    return xx;
                }
                else
                {
                    return gridCol.Header.ToString();
                }
            }
            return headerForExport;
        }

        private static string[] GetPath(DataGridColumn gridCol) {
            string path = GetPathForExport(gridCol);
            if (string.IsNullOrEmpty(path))
            {
                if(gridCol is DataGridBoundColumn)
                {
                    Binding binding = (Binding)((DataGridBoundColumn)gridCol).Binding;
                    if (binding != null)
                    {
                        path = binding.Path.Path;
                    }
                } else
                {
                    path = gridCol.SortMemberPath;
                }
            }
            return string.IsNullOrEmpty(path) ? null : path.Split('.');
           
        }

        private static object GetValue(string[] path, object obj, string formatForExport) {
#if !SILVERLIGHT
            if(obj is DataRowView)
            {
                DataRowView row = obj as DataRowView;
                string p = string.Join(".", path);
                obj = row[p];
            } else
#endif
            {
                foreach (string pathStep in path)
                {
                    if (obj == null)
                        return null;

                    Type type = obj.GetType();  
                    PropertyInfo property = type.GetProperty(pathStep);

                    if (property == null)
                    {
                        Debug.WriteLine(string.Format("Propiedad no encontrada '{0}' en el tipo '{1}'", pathStep, type.Name));
                        return null;
                    }

                    obj = property.GetValue(obj, null); 
                }
            }

            if (obj is System.DateTime)
            {
                System.DateTime date1 = (System.DateTime)obj;
                if(date1.TimeOfDay.TotalSeconds == 0)
                {
                    formatForExport = dateForm;
                } else
                {
                    formatForExport = dateForm;
                }
               
            }

            if(!string.IsNullOrEmpty(formatForExport))
            {
                return string.Format("{0:" + formatForExport + "}", obj);
            }

            return obj;
        }
    }
}
