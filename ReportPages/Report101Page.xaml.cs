using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls;

namespace ReportPages
{
    /// <summary>
    /// Interaction logic for Report10_1Page.xaml
    /// </summary>
    public partial class Report101Page : Page, IFillGrid
    {
        private static int index = 2;
        private bool isMouseDown;
        public Report101Page()
        {
            InitializeComponent();
        }

        public void FillGrid(string[,] output)
        {
            /*OutputGrid.DataSource = new TwoDimensionalArrayAdapter(output);
            for (var i = 0; i < OutputGrid.Columns.Count; i++)
            {
               // var column = BandsGrid.Columns[i];
                OutputGrid.Columns[i].Width = index;
                index += 10;
                
                
            }*/
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
    public class TwoDimensionalArrayAdapter : IList, ITypedList
    {
        string[,] array;
        PropertyDescriptorCollection columnCollection;
        public TwoDimensionalArrayAdapter(string[,] array)
        {
            this.array = array;
            CreateColumnCollection();
        }
        protected virtual void CreateColumnCollection()
        {
            VirtualPropertyDescriptor[] pds = new VirtualPropertyDescriptor[array.GetLength(1)];
            for (int n = 0; n < array.GetLength(1); n++)
            {
                pds[n] = new VirtualPropertyDescriptor(this, n, (n + 1).ToString(), typeof(float));
            }
            columnCollection = new PropertyDescriptorCollection(pds);
        }
        public void SetPropertyValue(int rowIndex, int columnIndex, string value)
        {
            array[rowIndex, columnIndex] = (string)value;
        }
        public string GetPropertyValue(int rowIndex, int columnIndex)
        {
            return array[rowIndex, columnIndex];
        }

        #region IList Members
        public virtual int Count
        {
            get { return array.GetLength(0); }
        }
        public virtual bool IsSynchronized
        {
            get { return true; }
        }
        public virtual object SyncRoot
        {
            get { return true; }
        }
        public virtual bool IsReadOnly
        {
            get { return false; }
        }
        public virtual bool IsFixedSize
        {
            get { return true; }
        }
        public virtual IEnumerator GetEnumerator()
        {
            return null;
        }
        public virtual void CopyTo(System.Array array, int fIndex)
        {
        }
        public virtual int Add(object val)
        {
            throw new NotImplementedException();
        }
        public virtual void Clear()
        {
            throw new NotImplementedException();
        }
        public virtual bool Contains(object val)
        {
            throw new NotImplementedException();
        }
        public virtual int IndexOf(object val)
        {
            throw new NotImplementedException();
        }
        public virtual void Insert(int fIndex, object val)
        {
            throw new NotImplementedException();
        }
        public virtual void Remove(object val)
        {
            throw new NotImplementedException();
        }
        public virtual void RemoveAt(int fIndex)
        {
            throw new NotImplementedException();
        }
        object IList.this[int fIndex]
        {
            get { return fIndex; 
            }
            set { }
        }
        #endregion
        #region ITypedList Members
        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            return columnCollection;
        }
        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return string.Empty;
        }
        #endregion
    }
    public class VirtualPropertyDescriptor : PropertyDescriptor
    {
        string propertyName;
        Type propertyType;
        TwoDimensionalArrayAdapter list;
        int index;
        public VirtualPropertyDescriptor(TwoDimensionalArrayAdapter list, int index, string propertyName, Type propertyType)
            : base(propertyName, null)
        {
            this.propertyName = propertyName + " kj  kj";
            this.propertyType = propertyType;
            this.list = list;
            this.index = index;
        }
        public override bool CanResetValue(object component)
        {
            return false;
        }
        public override object GetValue(object component)
        {
            return list.GetPropertyValue((int)component, index);
        }
        public override void SetValue(object component, object val)
        {
            val ??= string.Empty;
            list.SetPropertyValue((int)component, index, (string)val.ToString());
        }
        public override bool IsReadOnly { get { return false; } }
        public override string Name { get { return propertyName; } }
        public override Type ComponentType { get { return typeof(TwoDimensionalArrayAdapter); } }
        public override Type PropertyType { get { return propertyType; } }
        public override void ResetValue(object component)
        {
        }
        public override bool ShouldSerializeValue(object component) { return true; }
    }
}