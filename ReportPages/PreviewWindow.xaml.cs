using System.IO;
using System.Windows;
using DevExpress.Export.Xl;
using DevExpress.Spreadsheet;

namespace ReportPages
{
    public partial class PreviewWindow : Window
    {
        public PreviewWindow()
        {
            InitializeComponent();
        }

        public PreviewWindow(string fileName)
        {
            InitializeComponent();
            spreadsheetControl1.LoadDocument(fileName, DocumentFormat.Xlsx);
        }

        public void LoadDocument(string fileName)
        {
            spreadsheetControl1.LoadDocument(fileName, DocumentFormat.Xlsx);
        }
    }
}