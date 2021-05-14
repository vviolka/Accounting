using System.Windows.Controls;
using ReportPages;

namespace SalaryPages
{
    public partial class ReportCard : Page, IFillGrid
    {
        public ReportCard()
        {
            InitializeComponent();
        }
        public void FillGrid(string[,] output)
        {
            //OutputGrid.DataSource = new TwoDimensionalArrayAdapter(output);
        }

    }
}