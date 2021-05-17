using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model.DBStructure;

namespace Model
{
    public class ProductionsDB
    {
        private ApplicationContext ac;
        public ProductionsDB()
        {
            ac = new ApplicationContext();
        }

        public void Add(Production production)
        {
            ac.Productions.Add(production);
            ac.SaveChanges();
        }

        public void Update(Production oldProduction, Production newProduction)
        {
           Production current = ac.Productions.First(production => production.Id == oldProduction.Id);
           current.Name = newProduction.Name;
           current.Aluminum = newProduction.Aluminum;
           current.Cost = newProduction.Cost;
           current.Cullet = newProduction.Cullet;
           current.Date = newProduction.Date;
           current.Overhead = newProduction.Overhead;
           current.Steel = newProduction.Steel;
           current.Transportation = newProduction.Transportation;
           current.DocumentNumber = newProduction.DocumentNumber;
           current.PartnerId = newProduction.PartnerId;
           current.SellingPrice = newProduction.SellingPrice;
           current.HoursForProd = newProduction.HoursForProd;
           ac.SaveChanges();
        }

        public void Delete(Production production)
        {
            ac.Productions.Remove(production);
            ac.SaveChanges();
        }

        public List<ExcelExpence> GetExpences(Production production)
        {
            IQueryable<Expence> list = ac.Expences.Where(expence => expence.ProductionId == production.Id);
            DbSet<MaterialsInfo> infoes = ac.MaterialInfoes;
            IQueryable<ExcelExpence> result = from expence in list
                join info in infoes on expence.MaterialId equals info.Id
                select new ExcelExpence()
                {
                    Id = expence.Id,
                    Name = info.Name,
                    Cost = expence.Cost,
                    Count = expence.Count,
                    Price = info.Price,
                    Unity = info.Unity,
                    ProdCount = expence.Count - expence.CountForInstall,
                    InstallCount = expence.CountForInstall
                };
            return result.ToList();
        }

        public void ChangeExtraCount(int expenceId, float installCount)
        {
            ac.Expences.First(x => x.Id == expenceId).CountForInstall = installCount;
            ac.SaveChanges();
        }

      
    }
}