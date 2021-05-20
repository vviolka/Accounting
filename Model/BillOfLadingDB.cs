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
     //   private const int LevensteinDist = 2;
        /*public IEnumerable SearchByName(string name)
        {
            var resultList = new List<(BillOfLading, int)>();
            Parallel.ForEach(ac.BillOfLading.ToList(), (billOfLading) =>
            {
                int dist = LevenshteinDistance(Cut(billOfLading.Name, name.Length), name);
                if (dist >= LevensteinDist) return;
                lock (resultList)
                    resultList.Add((billOfLading, dist));
            });
            resultList.Sort((x, y) => -y.Item2.CompareTo(x.Item2));
            return resultList.ConvertAll(input => input.Item1);

        }*/
        public int Add(BillOfLading billOfLadings)
        {
            using var dc = new ApplicationContext();
            dc.BillsOfLading.Add(billOfLadings);
            dc.SaveChanges();

           int dateRowsCount = (dc.PartnersBalances.Where(x => x.PartnerId ==
                                            billOfLadings.PartnerId &&
                                            x.Date.Month == billOfLadings.Date.Month &&
                                            x.Date.Year == billOfLadings.Date.Year)).Count();

           int partnersRowsCount = (dc.PartnersBalances.Where(x => x.PartnerId == billOfLadings.PartnerId && x.Date < billOfLadings.Date)).Count();

           DateTime newDate = billOfLadings.Date;

           if (dateRowsCount > 0)
           {
               BillOfLading balance = ac.BillsOfLading.First(x => x.PartnerId == billOfLadings.PartnerId &&
                                                                  x.Date.Month == billOfLadings.Date.Month &&
                                                                  x.Date.Year == billOfLadings.Date.Year);
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
                       Date = newDate,
                       PartnerId = billOfLadings.PartnerId,
                       Sum = 0
                   });
               }
               else
               {
                   ac.PartnersBalances.Add(new PartnersBalances()
                   {
                       Date = (DateTime) NDate,
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

        public BillOfLading GetBillOfLading(int id)
        {
            using var applicationContext = new ApplicationContext();
            if (id == 0)
                return null;
            var billOfLading = this.ac.BillsOfLading.First(b => b.Id == id);

            return billOfLading;
        }

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
                    Partner = p.UNP
                }).ToList();
           
            return billOfLadings;
        }

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
        private static string Cut(string lastName, int n)
        {
            return lastName.Length < n ? lastName : lastName.Substring(0, n);
        }

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
        /*public IEnumerable SearchByName(string name)
                {
                    var resultList = new List<(Entrant, int)>();
                    var d1 = DateTime.Now;
                    Parallel.ForEach(EntrantsList, (entrant) =>
                    {
                        int dist = LevenshteinDistance(Cut(entrant.LastName, name.Length), name);
                        if (dist >= LevensteinDist) return;
                        lock (resultList)
                                resultList.Add((entrant, dist));
                    });
                    resultList.Sort((x, y) => -y.Item2.CompareTo(x.Item2));
                    var d2 = DateTime.Now;
                    Debug.WriteLine(d2 - d1);
                    return resultList.ConvertAll(input => input.Item1);
                }*/
        /*private static int LevenshteinDistance(string string1, string string2)
        {
            if (string1 == null) throw new ArgumentNullException();
            if (string2 == null) throw new ArgumentNullException();
            var m = new int[string1.Length + 1, string2.Length + 1];

            for (var i = 0; i <= string1.Length; i++) m[i, 0] = i;
            for (var j = 0; j <= string2.Length; j++) m[0, j] = j;

            for (var i = 1; i <= string1.Length; i++)
                for (var j = 1; j <= string2.Length; j++)
                {
                    int diff = char.ToLower(string1[i - 1]) == char.ToLower(string2[j - 1]) ? 0 : 1;
                    m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1,
                            m[i, j - 1] + 1),
                        m[i - 1, j - 1] + diff);
                }
            return m[string1.Length, string2.Length];
        }*/
    }
}
