using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model.DBStructure;

namespace Model
{
    public class BillOfLadingDB
    {
        private ApplicationContext ac;
        public BillOfLadingDB()
        {
            ac = new ApplicationContext();
        }
    
        //добавление записи в БД 
        public int Add(BillOfLading billOfLadings)
        {
            using var dc = new ApplicationContext();
            dc.BillsOfLading.Add(billOfLadings);
            dc.SaveChanges();

           int dateRowsCount = (dc.PartnersBalances.Where(x => x.PartnerId ==
                                            billOfLadings.PartnerId &&
                                            x.Date.Month == billOfLadings.Date.Month &&
                                            x.Date.Year == billOfLadings.Date.Year)).Count();

           int partnersRowsCount = (dc.PartnersBalances.Where(x => x.PartnerId == billOfLadings.PartnerId && x.Date < billOfLadings.Date.AddMonths(1))).Count();

           DateTime newDate = billOfLadings.Date;

           if (dateRowsCount > 0)
           {
               BillOfLading balance = ac.BillsOfLading.First(x => x.PartnerId == billOfLadings.PartnerId &&
                                                                  x.Date.Month == billOfLadings.Date.AddMonths(1).Month &&
                                                                  x.Date.Year == billOfLadings.Date.AddMonths(1).Year);
               balance.Date = billOfLadings.Date;
           }

           if (dateRowsCount == 0 && partnersRowsCount > 0)
           {
               DateTime? NDate = ac.PartnersBalances.Where(x => x.PartnerId == billOfLadings.PartnerId &&
                                                                x.Date < newDate).Max(y => y.Date);
               if (NDate == null)
               {
                   ac.PartnersBalances.Add(new PartnersBalances()
                   {
                       Date = newDate.AddMonths(1),
                       PartnerId = billOfLadings.PartnerId,
                       Sum = 0
                   });
               }
               else
               {
                   ac.PartnersBalances.Add(new PartnersBalances()
                   {
                       Date = (DateTime) newDate,
                       PartnerId = billOfLadings.PartnerId,
                       Sum = ac.PartnersBalances.First(x => x.Date == NDate &&
                                                            x.PartnerId == billOfLadings.PartnerId).Sum
                   });
               }

               ac.SaveChanges();
           }

           float sDebit = 0;
               if (billOfLadings.Type == "п/п")
               {
                   sDebit = billOfLadings.Cost;
               }

               if (partnersRowsCount > 0)
               {
                   var tmp = ac.PartnersBalances.Where(x => x.PartnerId == billOfLadings.PartnerId &&
                                                            x.Date >= newDate).ToList();
                   foreach (PartnersBalances balances in tmp)
                   {
                       balances.Sum = balances.Sum + sDebit - billOfLadings.Cost - billOfLadings.Vat;
                   }

                   ac.SaveChanges();
               }

               if (partnersRowsCount == 0)
                   {
                       ac.PartnersBalances.Add(new PartnersBalances()
                       {
                           PartnerId = billOfLadings.PartnerId,
                           Date = newDate,
                           Sum = sDebit - billOfLadings.Cost - billOfLadings.Vat
                       });
                       ac.SaveChanges();
                   }
               

           //return dc.BillsOfLading.First(b => b.Number == billOfLadings.Number && b.Type == billOfLadings.Type).Id;
            return billOfLadings.Id;
        }

        
        //получить платежку
        public BillOfLading GetBillOfLading(int id)
        {
            using var applicationContext = new ApplicationContext();
            if (id == 0)
                return null;
            var billOfLading = this.ac.BillsOfLading.First(b => b.Id == id);

            return billOfLading;
        }

        //получить список всех платежек 
        public List<BillOfLading> GetList()
        {
            using var applicationContext = new ApplicationContext();
            var billOfLading = this.ac.BillsOfLading.ToList();
            return billOfLading;

        }
        public List<AdaptedDocument> GetJoinedList()
        {
            using var dc = new ApplicationContext();
      
            List<AdaptedDocument>? billOfLadings = dc.BillsOfLading.Join(
                dc.Partners,
                b => b.PartnerId,
                p => p.Id,
                
                (b, p) => new AdaptedDocument()
                {
                    Id = b.Id,
                    Date = b.Date,
                    Type = b.Type,  
                    Number = b.Number,
                    Partner = p.Name
                }).ToList();
           
            return billOfLadings;
        }

        //обновить запись
        public void Edit(int id, BillOfLading oldBillOfLadings)
        {
            using var dataContext = new ApplicationContext();

            BillOfLading old = ac.BillsOfLading.First(x => x.Id == id);
            var newBillOfLadings = dataContext.BillsOfLading.First(x => x.Id == id);
            newBillOfLadings.Date = oldBillOfLadings.Date;
            newBillOfLadings.Number = oldBillOfLadings.Number;
            newBillOfLadings.PartnerId = oldBillOfLadings.PartnerId;
            newBillOfLadings.Type = oldBillOfLadings.Type;
            var newB = newBillOfLadings;
            dataContext.SaveChanges();

            float newDebit = 0;
            float oldDebit = 0;
            if (newB.Type == "п/п") newDebit = newB.Cost;
            if (old.Type == "п/п") oldDebit = old.Cost;

            var balances = ac.PartnersBalances.Where(x =>
                x.PartnerId == old.PartnerId &&
                x.Date >= old.Date);


           

            int dateRowsCount = (ac.PartnersBalances.Where(x => x.PartnerId ==
                                           newB.PartnerId &&
                                           x.Date.Month == newB.Date.Month &&
                                           x.Date.Year == newB.Date.Year)).Count();

            int partnersRowsCount = (ac.PartnersBalances.Where(x =>
                x.PartnerId == newB.PartnerId)).Count();

            DateTime newDate = newB.Date;

            if (dateRowsCount > 0)
            {
                BillOfLading balance = ac.BillsOfLading.First(x => x.PartnerId == newB.PartnerId &&
                                                                   x.Date >= newDate);
                balance.Date = newB.Date;
            }

            if (dateRowsCount == 0 && partnersRowsCount > 0)
            {
                DateTime? NDate = ac.PartnersBalances.Where(x =>
                    x.PartnerId == newB.PartnerId &&
                    x.Date < newDate).Max(y => y.Date);
                if (NDate == null)
                {
                    ac.PartnersBalances.Add(new PartnersBalances()
                    {
                        Date = newDate,
                        PartnerId = newB.PartnerId,
                        Sum = 0
                    });
                }
                else
                {
                    ac.PartnersBalances.Add(new PartnersBalances()
                    {
                        Date = (DateTime) NDate,
                        PartnerId = newB.PartnerId,
                        Sum = ac.PartnersBalances.First(x => x.Date == NDate &&
                                                             x.PartnerId == newB.PartnerId).Sum
                    });
                }

                ac.SaveChanges();
            }


            if (partnersRowsCount > 0)
            {
                var tmp = ac.PartnersBalances.Where(x => x.PartnerId == newB.PartnerId &&
                                                         x.Date >= newDate).ToList();
                foreach (PartnersBalances balance in tmp)
                {
                    balance.Sum = balance.Sum + newDebit - newB.CostWithVat;
                }

                ac.SaveChanges();
            }

            if (partnersRowsCount == 0)
                    {
                        ac.PartnersBalances.Add(new PartnersBalances()
                        {
                            PartnerId = newB.PartnerId,
                            Date = newDate,
                            Sum = newDebit - newB.CostWithVat
                        });
                        ac.SaveChanges();
                    }
            
        }

        //удалить запись
        public void Delete(int billOfLadingsId)
        {
            using var dataContext = new ApplicationContext();
            var old = ac.BillsOfLading.First(x => x.Id == billOfLadingsId);
            var newBillOfLadings = dataContext.BillsOfLading.First(x => x.Id == billOfLadingsId);
            dataContext.BillsOfLading.Remove(newBillOfLadings);
            dataContext.SaveChanges();

            float sDebit = 0;

            if (old.Type == "п/п") sDebit = old.Cost;
            var balances = ac.PartnersBalances.Where(x =>
                x.PartnerId == old.PartnerId &&
                x.Date >= old.Date);
            foreach (var balance in balances)
            {
                balance.Sum = balance.Sum - sDebit + old.CostWithVat;
            }

            ac.SaveChanges();

        }

        private string DateConverter(DateTime date)
        {
            var result = date.Date.ToString().Split(' ', (char)StringSplitOptions.None)[0].Replace("/", ".");
            return result;
        }
       
        //поиск
        public List<AdaptedDocument> Search(DateTime date, string type, string number, string partnersUNP)
        {
          List<AdaptedDocument> resultList;
           using var dc = new ApplicationContext();
            {
                var partners = (from p in dc.Partners where (partnersUNP == "" || partnersUNP == p.UNP) select p.Id).ToList();
                var tmp = (from m in dc.BillsOfLading
                    where (
                        (DateConverter(date) == null || DateConverter(date) == m.Date.ToString()) &&
                            (type == "" || type == m.Type) &&
                            (number == "" || EF.Functions.Like(m.Number, "%" + number + "%"))
                            &&
                             (partnersUNP == "" || partners.IndexOf(m.PartnerId) >= 0))
                    select m);

                resultList = tmp.Join(dc.Partners,
                    b => b.PartnerId,
                    p => p.Id,
                    (b, p) => new AdaptedDocument()
                    {
                        Id = b.Id,
                        Date = b.Date,
                        Type = b.Type,
                        Number = b.Number,
                        Partner = p.UNP
                    }).ToList();
            }


            return resultList;
        }
    }
}
