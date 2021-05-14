using System;
using System.Collections.Generic;
using System.IO;

namespace Model
{
	public class InfoPageModel 
	{
		private string GetSeperatedName(string name, out string organizationForm)
        {
	        if (name == string.Empty || name == "")
	        {
		        organizationForm = "";
		        return string.Empty;
	        }
                
			organizationForm = name.Split(' ')[0];
			name = name.Replace(organizationForm, string.Empty);
			name = name.Replace("\"", string.Empty);

			if (name != string.Empty)
			{
				organizationForm = organizationForm.ToUpper();


				name = name.TrimStart();
				name = '\"' + name;
				name += '\"';
				name = ' ' + name;
			}
			else
			{
				name = organizationForm;
				organizationForm = string.Empty;
			}
			return name;
		}
		public string TransformShortName(string shortOrganizationName)
		{
			if (shortOrganizationName == null)
				return string.Empty;
			shortOrganizationName = GetSeperatedName(shortOrganizationName, out var organizationForm);
			if (organizationForm != string.Empty)
				shortOrganizationName.TrimStart();
			shortOrganizationName = organizationForm + shortOrganizationName;
			return shortOrganizationName;
		}
		public string GenerateLongName(string shortOrganizationName)
		{
			if (shortOrganizationName == null)
				return string.Empty;
			string longOrganizationName;


			Dictionary<string, string> formDictionary = new Dictionary<string, string>
			{
				{"ООО", "Общество с ограниченной ответственностью"},
				{"ОАО", "Открытое акционерное общество"},
				{"ЗАО", "Закрытое акционерное общество"},
				{"ИП", "Индивидуальный предприниматель"},
				{"ОДО", "Общество с дополнительной ответственностью"},
				{"КТ", "Коммандитное товарищество"},
				{"ПТ", "Полное товарищество"},
				{"ПК", "Производственный кооператив"},
				{"УП", "Унитарное предприятие"},
				{"КФХ", "Крестьянское (фермерское) хозяйство"}
			};
			shortOrganizationName = GetSeperatedName(shortOrganizationName, out string organizationForm);
			if (formDictionary.TryGetValue(organizationForm, out var fullOrganizationForm))
			{
				//shortOrganizationName = organizationForm + name;
				longOrganizationName = fullOrganizationForm + ' ' + shortOrganizationName;
			}
			else
			{
				longOrganizationName = shortOrganizationName;
			}

			/*else if (name == String.Empty)
				longOrganizationName = organizationForm;
			else
				longOrganizationName = name;*/

			return longOrganizationName;
		}

		public void Save(string shortOrganizationName, string longOrganizationName, string unp, string egr, string registrationDate,
			string taxAuthority, string bankAccount, string head, string chiefAccountant, string cashier)
		{
			var path = Directory.GetCurrentDirectory() + "\\organization.dat";
			try
            {
                using var fileStream = new FileStream(path, FileMode.Create);
                using var streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(shortOrganizationName);
                streamWriter.WriteLine(longOrganizationName);
                streamWriter.WriteLine(unp);
                streamWriter.WriteLine(egr);
                streamWriter.WriteLine(registrationDate);
                streamWriter.WriteLine(taxAuthority);
                streamWriter.WriteLine(bankAccount);
                streamWriter.WriteLine(head);
                streamWriter.WriteLine(chiefAccountant);
                streamWriter.WriteLine(cashier);

                //SaveBtnVisible = false;
            }
			catch (IOException)
			{
				// ignored
			}
		}

		public bool Read(out string shortOrganizationName,
			out string longOrganizationName,
			out string unp,
			out string egr,
			out DateTime registrationDate,
			out string taxAuthority,
			out string bankAccount,
			out string head,
			out string chiefAccountant,
			out string cashier)
		{
			var path = Directory.GetCurrentDirectory() + "\\organization.dat";
			shortOrganizationName = null;
			longOrganizationName = null;
			unp = null;
			egr = null;
			registrationDate = default;
			taxAuthority = null;
			bankAccount = null;
			head = null;
			chiefAccountant = null;
			cashier = null;
			try
			{
				if (!File.Exists(path))
					return false;

                using var fileStream = new FileStream(path, FileMode.Open);
                using var streamReader = new StreamReader(fileStream);
                shortOrganizationName = streamReader.ReadLine();
                longOrganizationName = streamReader.ReadLine();
                unp = streamReader.ReadLine();
                egr = streamReader.ReadLine();
                registrationDate = Convert.ToDateTime(streamReader.ReadLine());
                taxAuthority = streamReader.ReadLine();
                bankAccount = streamReader.ReadLine();
                return true;
                //UpdateAll();
            }
			catch (IOException e)
			{
				Console.WriteLine(e);
				return false;
			}
		}
	}
}
