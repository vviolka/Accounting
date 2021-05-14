using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ReportPages
{
    public class Column
    {
        // Specifies the name of a data source field to which the column is bound. 
        
        public string Header { get; set; }
        public string FieldName { get; set; }
      
    }

    // Corresponds to a band column. 
    public class Band
    {
        // Specifies the band header.
        public string Header { get; set; }
        public string Fixed { get; set; }
        public ObservableCollection<Column> ChildColumns { get; set; }
    }
    public class BandTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var band = (Band)item;
            if (band.ChildColumns.Count == 1)
                return (DataTemplate)((Control)container).FindResource("SingleColumnBandTemplate");
            if (band.Fixed == "Right")
                return (DataTemplate) ((Control) container).FindResource("MultiColumnRightBandTemplate");
            if (band.Fixed=="Left")
                return (DataTemplate)((Control)container).FindResource("MultiColumnLeftBandTemplate");
            return (DataTemplate)((Control)container).FindResource("MultiColumnBandTemplate");

        
     
        }
    }
}