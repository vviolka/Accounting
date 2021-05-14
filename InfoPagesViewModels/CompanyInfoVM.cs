using System;
using DevExpress.Mvvm;
using Model;


namespace InfoPagesViewModels
{
	public class CompanyInfoVM : BindableBase
	{
		#region organizationName
		private string organizationName;
		public string OrganizationName
		{
			get => organizationName;
			set
			{
				organizationName = value;
				RaisePropertyChanged(nameof(organizationName));
				model.Save(shortOrganizationName, longOrganizationName, unp, egr, registrationDate.ToString(),
                    taxAuthority, bankAccount, head, chiefAccountant, cashier);
			}
		}

		#endregion


		#region shortOrganizationName
		private string shortOrganizationName;
		public string ShortOrganizationName
		{
			get => model.TransformShortName(shortOrganizationName);
			set
			{
                RaisePropertyChanged(nameof(organizationName));
				RaisePropertyChanged(nameof(shortOrganizationName));
				shortOrganizationName = value;
				RaisePropertyChanged(nameof(longOrganizationName));
				shortOrganizationName = model.TransformShortName(value);
                model.Save(shortOrganizationName, longOrganizationName, unp, egr, registrationDate.ToString(),
                    taxAuthority, bankAccount, head, chiefAccountant, cashier);

			}
		}

		#endregion

		#region longOrganizationName
		private string longOrganizationName;
		public string LongOrganizationName
		{
			get => model.GenerateLongName(shortOrganizationName);
			set
			{
                longOrganizationName = value;
                model.Save(shortOrganizationName, longOrganizationName, unp, egr, registrationDate.ToString(),
                    taxAuthority, bankAccount, head, chiefAccountant, cashier);

			}
		}
		#endregion

		#region unp
		private string unp;
		public string UNP
		{
			get => unp;
			set
			{
				unp = value;
				egr = unp;
				RaisePropertyChanged(nameof(egr));
				RaisePropertyChanged(nameof(unp));
                model.Save(shortOrganizationName, longOrganizationName, unp, egr, registrationDate.ToString(),
                    taxAuthority, bankAccount, head, chiefAccountant, cashier);
			}
		}


		#endregion

		#region egr
		private string egr;
		public string EGR
		{
			get => egr;
			set
			{
                egr = value;
				RaisePropertyChanged(nameof(egr));
                model.Save(shortOrganizationName, longOrganizationName, unp, egr, registrationDate.ToString(),
                    taxAuthority, bankAccount, head, chiefAccountant, cashier);
			}
		}
		#endregion

		#region registrationDate
		private DateTime registrationDate;
		public DateTime RegistrationDate
		{
			get => registrationDate;
			set
			{
                registrationDate = value;
				RaisePropertyChanged(nameof(registrationDate));
                model.Save(shortOrganizationName, longOrganizationName, unp, egr, registrationDate.ToString(),
                    taxAuthority, bankAccount, head, chiefAccountant, cashier);
			}
		}
		#endregion

		#region taxAuthority
		private string taxAuthority;
		public string TaxAuthority
		{
			get => taxAuthority;
			set
			{
				taxAuthority = value;
				RaisePropertyChanged(nameof(taxAuthority));
                model.Save(shortOrganizationName, longOrganizationName, unp, egr, registrationDate.ToString(),
                    taxAuthority, bankAccount, head, chiefAccountant, cashier);
			}
		}
		#endregion

		#region bankAccount
		private string bankAccount;
		public string BankAccount
		{
			get => bankAccount;
			set
			{
				bankAccount = value.ToUpper();
				RaisePropertyChanged(nameof(bankAccount));
                model.Save(shortOrganizationName, longOrganizationName, unp, egr, registrationDate.ToString(),
                    taxAuthority, bankAccount, head, chiefAccountant, cashier);
			}
		}

		#endregion

		#region head
		private string head;
		public string Head
		{
			get => head;
			set => head = value;
		}
		#endregion

		#region chiefAccountant
		private string chiefAccountant;
		public string ChiefAccountant
		{
			get => chiefAccountant;
			set => chiefAccountant = value;
		}
		#endregion

		#region cashier
		private string cashier;
		public string Cashier
		{
			get => cashier;
			set => cashier = value;
		}
		#endregion

		

		#region infoPageLoad
		private DelegateCommand infoPageLoaded;
		public DelegateCommand InfoPageLoaded
		{
			get => infoPageLoaded;
			set => infoPageLoaded = value;
		}
		public void ReadFile()
        {
            Console.WriteLine("in read file func");
            model.Read(out shortOrganizationName, out longOrganizationName, out unp, out egr,
            	out registrationDate,
            	out taxAuthority, out bankAccount, out head, out chiefAccountant, out cashier);
            UpdateAll();
        }

		#endregion

		#region helpsVoids
		private void UpdateAll()
		{
			RaisePropertyChanged(nameof(organizationName));
			RaisePropertyChanged(nameof(shortOrganizationName));
			RaisePropertyChanged(nameof(longOrganizationName));
			RaisePropertyChanged(nameof(unp));
			RaisePropertyChanged(nameof(egr));
			RaisePropertyChanged(nameof(registrationDate));
			RaisePropertyChanged(nameof(taxAuthority));
			RaisePropertyChanged(nameof(bankAccount));
		}

        #endregion

		#region constructor

		private InfoPageModel model;
		public CompanyInfoVM()
		{
            infoPageLoaded = new DelegateCommand(ReadFile);
			model = new InfoPageModel();

		}

		#endregion
	}
}
