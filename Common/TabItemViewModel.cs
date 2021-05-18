using System;

namespace Common
{
    public class TabItemViewModel
    {
        public TabItemViewModel(string header, object content, Action<TabItemViewModel> onClose)
        {
            this.Header = header;
            this.Content = content;
            this.CloseCommand = new RelayCommand(() => onClose(this));
        }
        public string Header { get; set; }
        public object Content { get; set; }

        public RelayCommand CloseCommand { get; set; }
    }
}