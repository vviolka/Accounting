using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace DocumentsPages
{
    /// <summary>
    /// Interaction logic for TTN.xaml
    /// </summary>
    public partial class TTN : Page
    {
        public TTN()
        {
            InitializeComponent();
            grid.Columns[0].Width = grid.Columns[0].ActualWidth;
        }
    }
}
