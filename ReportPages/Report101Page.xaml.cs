using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace ReportPages
{
    /// <summary>
    /// Interaction logic for Report10_1Page.xaml
    /// </summary>
    public partial class Report101Page : Page, IFillGrid
    {
        private IPageActions pageActions;
        private static int index = 2;
        private bool isMouseDown;
        public Report101Page()
        {
            InitializeComponent();
        }
        
        public Report101Page(IPageActions pageActions)
        {
            InitializeComponent();
            this.pageActions = pageActions;
        }

        public void FillGrid(string[,] output)
        {
           
        }

     

        /*private void TableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            
        }

        private void BandsGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            for (var i = 0; i < BandsGrid.Columns.Count; i++)
            {
                var column = BandsGrid.Columns[i];
                OutputGrid.Columns[i].Width = column.Width;
            }
        }

        private void TableView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MessageBox.Show("j");
        }

        private void BandsGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
        }

        private void BandsGrid_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
        }

        private void BandsGrid_OnMouseMove(object sender, MouseEventArgs e)
        {
          //  if (isMouseDown)
         //   {
                /*for (var i = 0; i < BandsGrid.Columns.Count; i++)
                {
                    var column = BandsGrid.Columns[i];
                    if (OutputGrid.Columns[i].ActualAdditionalRowDataWidth != column.ActualAdditionalRowDataWidth)
                    { 
                    ((GridColumn)OutputGrid.Columns[i]).Width = column.ActualAdditionalRowDataWidth;}
                }#1#
           //}
        }

        private void TableView_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void OutputGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void TableView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void TableView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }*/

    }
    
}