using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Common;
using DevExpress.Mvvm;
using LogPages;
using Model;

namespace LogPagesViewModels
{
    public class GenerateLogVM
    {
        private DateTime logDate;
        private string logAccount;
        private IWindow window;
        public GenerateLogVM()
        {
            generateLogCommand = new DelegateCommand(GenerateLog);
            accounts = (List<string>) Lists.GetLogAccounts();
        }


        #region Account

        private int account;

        public int Account
        {
            get => account;
            set => account = value;
        }

        #endregion

        #region Date

        private DateTime date;

        public DateTime Date
        {
            get => date;
            set => date = value;
        }

        #endregion

        #region Accounts

        public List<string> Accounts
        {
            get => accounts;
            set => accounts = value;
        }
        private List<string> accounts;

        #endregion
        #region GenerateLog

        private ICommand generateLogCommand;

        public ICommand GenerateLogCommand
        {
            get => generateLogCommand;
            set => generateLogCommand = value;
        }

        private void GenerateLog()
        {
            logAccount = Accounts[account];
            logDate = date;
            window.Close();

        }
        #endregion

       
        public void Show(out string account, out DateTime date)
        {
            window = new GenerateLogWindow(){DataContext = this};
            var t = window.ShowDialog();
            account = logAccount;
            date = logDate;
        }
    }
}
