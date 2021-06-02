using System;
using System.Collections.Generic;
using System.Windows.Input;
using DevExpress.Mvvm;
using Model;
using SalaryPages;

namespace SalaryPagesViewModels
{
    public class AddReportVM
    {
       #region Date

        private DateTime reportDate;

        public DateTime ReportDate
        {
            get => reportDate;
            set => reportDate = value;
        }

        #endregion
        public void Show(out DateTime date)
        {
            window = new AddCardReport(){DataContext = this};
            bool? t = window.ShowDialog();
            date = reportDate;
            
        }
        public AddReportVM()
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
            reportDate = date;
            window.Close();

        }
        #endregion
        private AddCardReport window;
    }
}