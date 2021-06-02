using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Common;
using DevExpress.Mvvm;
using Model;
using Model.DBStructure;
using ReportPages;
using ReportPagesViewModels.Annotations;


namespace ReportPagesViewModels
{
    class CalculationSettingsVM: BindableBase
    {
        private ProductionsDB model;
        private Production production;
        private CalculationSettingsWindow window;
        private List<ExcelExpence> allExcpences;
        public CalculationSettingsVM()
        {
            model = new ProductionsDB();
            
            
            
            deleteCommand = new DelegateCommand(Delete);
            saveCommand = new DelegateCommand(Save);
            createCommand = new DelegateCommand(Create);
            window = new CalculationSettingsWindow(){DataContext = this};
        }

        public Production OpenForCreate()
        {
            createPanelVisible = true;
            editPanelVisible = false;
            window.ShowDialog();
            date = DateTime.Today;
            Date = DateTime.Now;
            
            return production;
        }
        public void Show(Production production)
        {
            createPanelVisible = false;
            editPanelVisible = true;
            this.production = production;
            allExcpences = model.GetExpences(production);
            installExpencesList = allExcpences.ToList();
            RaisePropertiesChanged(nameof(ProductionExpencesList));
            Name = production.Name;
            SellingPrice = production.Cost;
            Partner = partners.IndexOf(partners.First(partner => partner.Id == production.PartnerId));
            DocumentNumber = production.DocumentNumber;
            Date = production.Date;
            Cullet = production.Cullet;
            Steel = production.Steel;
            Aluminum = production.Aluminum;
            Overhead = production.Overhead;
            Transportation = production.Transportation;
            SellingPrice = production.SellingPrice;
            HoursForProd = int.Parse(production.HoursForProd.ToString());
            window.ShowDialog();
        }

        #region Name

        private string name;

        public string Name
        {
            get => name;
            set => SetProperties.SetProperty(ref name, value);
        }

        private ICommand nameChangedCommand;

        public ICommand NameChangedCommand
        {
            get => nameChangedCommand;
            set => nameChangedCommand = value;
        }
        #endregion

        #region DocumentNumber

        private string documentNumber;

        public string DocumentNumber
        {
            get => documentNumber;
            set => SetProperties.SetProperty(ref documentNumber, value);
        }

        #endregion


        #region Date

        private DateTime date;

        public DateTime Date
        {
            get => date;
            set => SetProperties.SetProperty(ref date, value);
        }

        #endregion

        #region Partner

        private int partner;

        public int Partner
        {
            get => partner;
            set => SetProperties.SetProperty(ref partner, value);
        }

        #endregion

        #region  Cullet

        private float cullet;

        public float Cullet
        {
            get => cullet;
            set => SetProperties.SetProperty(ref cullet, value);
        }

        #endregion

        #region Steel

        private float steel;

        public float Steel
        {
            get => steel;
            set => SetProperties.SetProperty(ref steel, value);
        }

        #endregion

        #region Aluminum

        private float aluminum;

        public float Aluminum
        {
            get => aluminum;
            set => SetProperties.SetProperty(ref aluminum, value);
        }

        #endregion

        #region Overhead

        private float overhead;

        public float Overhead
        {
            get => overhead;
            set => SetProperties.SetProperty(ref overhead, value);
        }

        #endregion

        #region Transporation

        private float transportation;

        public float Transportation
        {
            get => transportation;
            set => SetProperties.SetProperty(ref transportation, value);
        }

        #endregion

        #region SellingPrice

        private float sellingPrice;

        public float SellingPrice
        {
            get => sellingPrice;
            set => SetProperties.SetProperty(ref sellingPrice, value);
        }

        #endregion

        #region HoursForProd

        private int hoursForProd;

        public int HoursForProd
        {
            get => hoursForProd;
            set => SetProperties.SetProperty(ref hoursForProd, value);
        }

        #endregion

        #region EditPanelVisible

        private bool editPanelVisible;

        public bool EditPanelVisible
        {
            get => editPanelVisible;
            set => SetProperties.SetProperty(ref editPanelVisible, value);
        }

        #endregion

        #region CreatePanelVisible

        private bool createPanelVisible;

        public bool CreatePanelVisible
        {
            get => createPanelVisible;
            set => SetProperties.SetProperty(ref createPanelVisible, value);
        }

        #endregion
        #region Save

        private ICommand saveCommand;

        public ICommand SaveCommand
        {
            get => saveCommand;
            set => saveCommand = value;
        }

        private void Save()
        {
            var prod = new Production()
            {
                Name = name, Aluminum = Aluminum, Cost = 0, SellingPrice = SellingPrice, Cullet = Cullet, Date = Date,
                DocumentNumber = DocumentNumber, HoursForProd = HoursForProd, Overhead = Overhead, Steel = Steel,
                Transportation = Transportation, PartnerId = partners[Partner].Id
            };
            
            model.Update(production, prod);

            foreach (ExcelExpence expence in installExpencesList)
            {
                model.ChangeExtraCount(expence.Id, expence.InstallCount);
            }
            
            window.Close();
            

        }

        #endregion

        #region Create

        private ICommand createCommand;

        [NotNull]
        public ICommand CreateCommand => createCommand;

        private void Create()
        {
            production = new Production()
            {
                Aluminum = Aluminum,
                Steel = Steel,
                Cullet = Cullet,
                Date = Date,
                DocumentNumber = DocumentNumber,
                HoursForProd = HoursForProd,
                Name = Name,
                Overhead = Overhead,
                PartnerId = partners[Partner].Id,
                Transportation = Transportation,
                SellingPrice = SellingPrice
            };
            new Reports().AddProduction(production);

            window.Close();
        }

        #endregion

        #region Delete

        private ICommand deleteCommand;

        public ICommand DeleteCommand => deleteCommand;

        private void Delete()
        {
            model.Delete(production);
            window.Close();
        }

        #endregion

        #region Lists

        private List<Partner> partners = new PartnerDB().GetList();

        public List<string> Partners => partners.Select(partner => partner.Name).ToList();
        private List<ExcelExpence> productionExpencesList;
        private List<ExcelExpence> installExpencesList;

        public List<ExcelExpence> ProductionExpencesList
        {
            get => productionExpencesList;
            set => productionExpencesList = value;
        }

        public List<ExcelExpence> InstallExpencesList
        {
            get => installExpencesList;
            set => installExpencesList = value;
        }

        #endregion
        
        

        

        
    }
}
