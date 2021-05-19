using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Common;
using DevExpress.Mvvm;
using DocumentsPages;
using Model;
using Model.DBStructure;

namespace DocumentsPagesViewModels
{
    public class PaymentCardVM: BindableBase
    {
        private BillOfLading? billOfLading;
        private BillOfLadingDB model;
        private TTNVM.DeleteHandler deleteHandler;
        private TTNVM.DeleteHandler updateHandler;

        public PaymentCardVM(BillOfLading? billOfLading,  TTNVM.DeleteHandler notifier, TTNVM.DeleteHandler updater)
        {
            model = new BillOfLadingDB();
            deleteHandler = notifier;
            updateHandler = updater;
            this.billOfLading = billOfLading;
            saveCommand = new DelegateCommand(Save);
            deleteCommand = new DelegateCommand(Delete);
            partners = new PartnerDB().GetList();

            if (billOfLading == null) 
                return;
            SelectedPartner = partners.IndexOf(partners.First(partner => partner.Id == billOfLading.PartnerId));
            Cost = billOfLading.Cost;
            Date = billOfLading.Date;
            Number = billOfLading.Number;
            Purpose = this.billOfLading.Purpose;

        }

        #region Lists

        private List<Partner> partners = new PartnerDB().GetList();
        public List<string> Partners => partners.Select(partner => partner.Name).ToList();

        #endregion

        #region SelectedPartner

        private int selectedPartner;

        public int SelectedPartner
        {
            get => selectedPartner;
            set
            {
                SetProperties.SetProperty(ref selectedPartner, value);
                Check();
                RaisePropertiesChanged();
            }
        }

        #endregion

        #region Date

        private DateTime date;

        public DateTime Date
        {
            get => date;
            set
            {
                SetProperties.SetProperty(ref date, value);
                Check();
                RaisePropertiesChanged();
            }
        }

        #endregion

        #region Cost

        private float cost;

        public float Cost
        {
            get => cost;
            set
            {
                SetProperties.SetProperty(ref cost, value);
                Check();
                RaisePropertiesChanged();
            }
        }

        #endregion

        #region Number

        private string number;

        public string Number
        {
            get => number;
            set
            {
                SetProperties.SetProperty(ref number, value);
                Check();
                RaisePropertiesChanged();
            }
        }

        #endregion

        #region Purposee

        private string purpose;

        public string Purpose
        {
            get => purpose;
            set  
            { 
                SetProperties.SetProperty(ref purpose, value);
                Check();
                RaisePropertiesChanged();
            }
        }

        #endregion

        #region SaveCommand

        private ICommand saveCommand;

        public ICommand SaveCommand
        {
            get => saveCommand;
            set => saveCommand = value;
        }

        private void Save()
        {
            
            BillOfLading newBillOfLading = new BillOfLading()
            {
                Cost = Cost,
                Date = Date,
                PartnerId = partners[selectedPartner].Id,
                Number = Number,
                Purpose = purpose,
                Type="п/п"
            };
            if (billOfLading == null)
                model.Add(newBillOfLading);
            else
                model.Edit(billOfLading.Id, newBillOfLading);
           // updateHandler?.Invoke();
        }

        #endregion

        #region DeleteCommand

        private ICommand deleteCommand;

        public ICommand DeleteCommand
        {
            get => deleteCommand;
            set => deleteCommand = value;
        }

        private void Delete()
        {
            model.Delete(billOfLading.Id);
            deleteHandler?.Invoke();
        }

        #endregion

        #region IsSaveButtonEnable

        private bool isEnable;

        public bool IsEnable
        {
            get => isEnable;
            set => SetProperties.SetProperty(ref isEnable, value);
        }

        private void Check()
        {
            IsEnable =  !(Number == string.Empty || Purpose == String.Empty || Cost == 0 || SelectedPartner == -1);
        }

        #endregion
    }
}