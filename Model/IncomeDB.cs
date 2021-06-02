using System.Collections.Generic;
using System.Linq;
using Model.DBStructure;

namespace Model
{
    public class IncomeDB
    {
        private ApplicationContext ac;

        public IncomeDB()
        {
            ac = new ApplicationContext();
        }


        //добавить запись
        public void Add(Income income)
        {
            using var dc = new ApplicationContext();
            income.CostWithVat = income.Cost + income.Vat;
            dc.Incomes.Add(income);
            dc.SaveChanges();

            var bill = ac.BillsOfLading.First(x => x.Id == income.BillId);
            var balance = ac.PartnersBalances.Where(x => x.PartnerId == bill.PartnerId && x.Date >= bill.Date);
            foreach (PartnersBalances balances in balance)
            {
                balances.Sum -= income.Cost - income.Vat;
            }

            ac.SaveChanges();
        }

        //получить лист материлов, пришедших по платежке 
        public List<Income> GetList(int billOfLadingId)
        {
            using var dc = new ApplicationContext();
            var income = dc.Incomes.Where(m => m.BillId == billOfLadingId).ToList();

            return income;
        }
        
        //редактирование
        public void Edit(int id, Income oldIncome)
        {
            using var dataContext = new ApplicationContext();
            var newIncome = dataContext.Incomes.First(x => x.Id == id);

            var bill = ac.BillsOfLading.First(x => x.Id == oldIncome.BillId);
            var balance = ac.PartnersBalances.Where(x => x.PartnerId == bill.PartnerId && x.Date >= bill.Date);
            foreach (PartnersBalances balances in balance)
            {
                balances.Sum += oldIncome.Cost  + oldIncome.Vat - newIncome.Cost - newIncome.Vat;
            }
            newIncome.Count = oldIncome.Count;
            newIncome.Cost = oldIncome.Cost;
            newIncome.VatRate = oldIncome.VatRate;
            newIncome.Vat = oldIncome.Vat;
            newIncome.Weight = oldIncome.Weight;
            

            dataContext.SaveChanges();


        }

        //удалить
        public void Delete(Income income)
        {
            using var dataContext = new ApplicationContext();
            var newIncome = dataContext.Incomes.First(x => x.Id == income.Id);
            dataContext.Incomes.Remove(newIncome);
            dataContext.SaveChanges();

            var bill = ac.BillsOfLading.First(x => x.Id == income.BillId);
            var balance = ac.PartnersBalances.Where(x => x.PartnerId == bill.PartnerId && x.Date >= bill.Date);
            foreach (PartnersBalances balances in balance)
            {
                balances.Sum += income.Cost + income.Vat;
            }

            dataContext.SaveChanges();
        }
        
    }
}