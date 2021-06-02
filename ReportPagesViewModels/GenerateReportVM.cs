using System;
using System.Collections.Generic;
using System.Windows.Input;
using DevExpress.Mvvm;
using Model;
using ReportPages;

namespace ReportPagesViewModels
{
    public class GenerateReportVM
    {
        private DateTime reportDate;
        private string reportAccount;
        private GenerateReportWindow window;
        public GenerateReportVM()
        {
            generateReportCommand = new DelegateCommand(GenerateReport);
            accounts = (List<string>) Lists.GetMaterialsAccounts();
            date = DateTime.Today;
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
        #region GenerateReport

        private ICommand generateReportCommand;

        public ICommand GenerateReportCommand
        {
            get => generateReportCommand;
            set => generateReportCommand = value;
        }

        private void GenerateReport()
        {
            reportAccount = Accounts[account];
            reportDate = date;
            window.Close();

        }
        #endregion

       
        public void Show(out string account, out DateTime date)
        {
            window = new GenerateReportWindow(){DataContext = this};
            var t = window.ShowDialog();
            account = reportAccount;
            date = reportDate;
        }
    }
}
