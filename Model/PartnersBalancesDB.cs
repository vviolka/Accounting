using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using Model.DBStructure;

namespace Model
{
    public class PartnersBalancesDB
    {
        private ApplicationContext ac;
        public PartnersBalancesDB()
        {
            ac = new ApplicationContext();
        }

        public List<PartnersBalances> GetList(DateTime date)
        {
            var balancesList = ac.PartnersBalances.Where(x => x.Date <= date).ToList();
            var balances = (from balance in balancesList
                where
                    balancesList.GroupBy(g => g.PartnerId)
                        .Select(s => new
                        {
                            PartnersId = s.Key,
                            MaxDate = s.Max(x => x.Date)
                        }).Contains(new {PartnersId = balance.PartnerId, MaxDate = balance.Date})
                select new PartnersBalances()
                {
                    Id = balance.Id,
                    Sum = balance.Sum,
                    Date = balance.Date,
                    PartnerId = balance.PartnerId
                });
            return balances.ToList();
        }

        public float GetCostForAccount(string account, int partnerId, DateTime date)
        {
             return (from bill in ac.BillsOfLading 
                 where bill.Type != "п/п" && bill.PartnerId == partnerId && date.Month == bill.Date.Month && date.Year == bill.Date.Year
                join income in ac.Incomes on bill.Id equals income.BillId
                join info in ac.MaterialInfoes on income.MaterialId equals info.Id where info.Account == account
                select new
                {
                    Cost = income.Cost
                }).Sum(x => x.Cost);
        }

        public float GetVatSum(int partnerId, DateTime date)
        {
            return (ac.BillsOfLading.Where(bill =>
                bill.Type != "п/п" && bill.PartnerId == partnerId && date.Month == bill.Date.Month &&
                date.Year == bill.Date.Year)).Sum(x => x.Vat);
        }
        
        public float GetCostSum(int partnerId, DateTime date)
        {
            return (ac.BillsOfLading.Where(bill =>
                bill.Type != "п/п" && bill.PartnerId == partnerId && date.Month == bill.Date.Month &&
                date.Year == bill.Date.Year)).Sum(x => x.Cost + x.Vat);
        }

        public (float, string) GetPaymentsInfo(int partnerId, DateTime date)
        {
           float cost = 0;
             string description = null!;
             foreach (var t in (from bill in ac.BillsOfLading
                 where bill.Type == "п/п"
                       && bill.PartnerId == partnerId
                       && date.Month == bill.Date.Month &&
                       date.Year == bill.Date.Year
                 select new
                 {
                     Cost = bill.Cost,
                     Description = $"{bill.Date.Day}.{bill.Date.Month}.{bill.Date.Year} №{bill.Number}"
                 }))
             {
                 cost += t.Cost;
                 description += t.Description + '\n';
             }

             return (cost, description)!;

        }
    }
}