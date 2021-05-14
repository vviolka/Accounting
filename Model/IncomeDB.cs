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


        public void Add(Income income)
        {
            using var dc = new ApplicationContext();
            dc.Incomes.Add(income);
            dc.SaveChanges();
        }

        public List<Income> GetList(int billOfLadingId)
        {
            using var dc = new ApplicationContext();
            var income = dc.Incomes.Where(m => m.BillId == billOfLadingId).ToList();

            return income;
        }
        
        public void Edit(int id, Income oldIncome)
        {
            using var dataContext = new ApplicationContext();
            var newIncome = dataContext.Incomes.First(x => x.Id == id);
            newIncome.Count = oldIncome.Count;
            newIncome.Cost = oldIncome.Cost;
            newIncome.VatRate = oldIncome.VatRate;
            newIncome.Vat = oldIncome.Vat;
            newIncome.Weight = oldIncome.Weight;
            

            dataContext.SaveChanges();
        }

        public void Delete(Income income)
        {
            using var dataContext = new ApplicationContext();
            var newIncome = dataContext.Incomes.First(x => x.Id == income.Id);
            dataContext.Incomes.Remove(newIncome);
            dataContext.SaveChanges();
        }
        
    }
}