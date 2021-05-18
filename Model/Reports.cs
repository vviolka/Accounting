using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using Model.DBStructure;
using Action = System.Action;

namespace Model
{
    public class Reports
    {
        public List<AvailableMaterials> GetAvailableMaterials(DateTime date, string account)
        {
            using var ac = new ApplicationContext();

            var incomes = ac.Incomes.ToList<Income>();
            var materials = ac.MaterialInfoes.Where(material => material.Account == account).ToList();
            var bills = ac.BillsOfLading.ToList();
            var balancesList = ac.Balances.ToList();
            var balances = (from balance in balancesList
                where
                    balancesList.GroupBy(g => g.MaterialId)
                        .Select(s => new
                        {
                            MaterialsId = s.Key,
                            MaxDate = s.Max(x => x.Date)
                        }).Contains(new {MaterialsId = balance.MaterialId, MaxDate = balance.Date})
                select new BalanceDto()
                {
                    Id = balance.Id,
                    Cost = balance.Cost,
                    Count = balance.Count,
                    Date = balance.Date,
                    MaterialsId = balance.MaterialId
                });
            var tmp = (from material in materials
                join income in incomes on material.Id equals income.MaterialId
                join bill in bills on income.BillId equals bill.Id
                
                select
                    new AvailableMaterials()
                    {
                        Id = material.Id,
                        Name = material.Name,
                        ICount = income.Count,
                        ICost = income.Cost,
                        Date = bill.Date,
                        Number = bill.Number,
                        Price = material.Price,
                        Unity = material.Unity,
                        Type = bill.Type
                    });
            var result = (from balance in balances
                join material in materials on balance.MaterialsId equals material.Id
                join income in incomes on material.Id equals income.MaterialId
                join bill in bills on income.BillId equals bill.Id
                where bill.Date < date
                          select
                    new AvailableMaterials()
                    {
                        Id = material.Id,
                        Name = material.Name,
                        Count = balance.Count,
                        Cost = balance.Cost,
                        Date = bill.Date,
                        Number = bill.Number,
                        Price = material.Price,
                        Unity = material.Unity,
                        Type = bill.Type
                    })
                .Concat(
                    from material in materials
                    join income in incomes on material.Id equals income.MaterialId
                    join bill in bills on income.BillId equals bill.Id
                    where bill.Date.Month == date.Month
                    select
                        new AvailableMaterials()
                        {
                            Id = material.Id,
                            Name = material.Name,
                            ICount = income.Count,
                            ICost = income.Cost,
                            Date = bill.Date,
                            Number = bill.Number,
                            Price = material.Price,
                            Unity = material.Unity,
                            Type = bill.Type
                        });
            return result.AsQueryable().ToList();
        }

        public List<Production> GetProductions(DateTime date)
        {
            using var ac = new ApplicationContext();
            var tmp = ac.Productions.ToList();
            return ac.Productions.Where(p => p.Date.Month == date.Month).ToList();
        }

        public List<Expence> GetExpences(int productionsId)
        {
            using var ac = new ApplicationContext();
            return ac.Expences.Where(expence => expence.ProductionId == productionsId).ToList();
        }

        public void InsertExpence(Expence expence)
        {
            try
            {
                using var ac = new ApplicationContext();

                ac.Expences.Add(expence);
                ac.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }

        public void UpdateExpence(int id, Expence newExpence)
        {
            using var ac = new ApplicationContext();
            Expence oldExpence = ac.Expences.First(expence => expence.Id == id);
            oldExpence.Cost = newExpence.Cost;
            oldExpence.Count = newExpence.Count;
            ac.SaveChanges();
        }

        
        public List<ExcelExpence> GetListForExcelReport(Production production)
        {
            using var ac = new ApplicationContext();
            List<ExcelExpence> excelExpences = (from expence in ac.Expences
                    where
                        expence.ProductionId == production.Id
                    join materialsInfo in ac.MaterialInfoes on expence.MaterialId equals materialsInfo.Id
                    select new ExcelExpence()
                    {
                        Name = materialsInfo.Name,
                        Unity = materialsInfo.Unity,
                        Count = expence.Count,
                        Price = materialsInfo.Price,
                        Cost = expence.Cost
                    }
                ).ToList();
            return excelExpences;

        }

        public Expence GetExpence(int materialsId, int productionId)
        {
            using var ac = new ApplicationContext();
            try
            {
                return ac.Expences.First(x => x.MaterialId == materialsId && x.ProductionId == productionId);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return null;
            }
           
        }
        public void Delete(int id)
        {
            using var ac = new ApplicationContext();
            var expence = ac.Expences.First(x => x.Id == id);
            ac.Expences.Remove(expence);
            ac.SaveChanges();
        }
        public void AddProduction(Production production)
        {
            using var ac = new ApplicationContext();
            ac.Productions.Add(production);
            ac.SaveChanges();
        }

        public float GetSumOfMaterials(Production production)
        {
            using var ac = new ApplicationContext();
            return ac.Expences.Where(expence => expence.ProductionId == production.Id).Sum(expence => expence.Cost);
        }
        
        public string GetPartnerName(int partnerId)
        {
            using var ac = new ApplicationContext();
            return ac.Partners.First(x => x.Id == partnerId).Name;
        }
    }
}
