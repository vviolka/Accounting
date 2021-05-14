/*
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportsPages
{
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
                pds[n] = new VirtualPropertyDescriptor(this, n, (n + 1).ToString(), typeof(int));
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
            get { return fIndex; }
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
}
*/
