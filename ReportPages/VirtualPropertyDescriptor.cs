/*
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportsPages
{
    public class VirtualPropertyDescriptor : PropertyDescriptor
    {
        string propertyName;
        Type propertyType;
        TwoDimensionalArrayAdapter list;
        int index;
        public VirtualPropertyDescriptor(TwoDimensionalArrayAdapter list, int index, string propertyName, Type propertyType)
            : base(propertyName, null)
        {
            this.propertyName = propertyName;
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
            list.SetPropertyValue((int)component, index, (string)val);
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
*/
