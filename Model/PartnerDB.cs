using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model.DBStructure;

// using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class PartnerDB
    {
        private ApplicationContext ac;
        public PartnerDB()
        {
            ac = new ApplicationContext();
        }
        private const int LevensteinDist = 2;
        public IEnumerable SearchByName(string name)
        {
            var resultList = new List<(Partner, int)>();
            Parallel.ForEach(ac.Partners.ToList(), (partner) =>
            {
                int dist = LevenshteinDistance(Cut(partner.Name, name.Length), name);
                if (dist >= LevensteinDist) return;
                lock (resultList)
                    resultList.Add((partner, dist));
            });
            resultList.Sort((x, y) => -y.Item2.CompareTo(x.Item2));
            return resultList.ConvertAll(input => input.Item1);

        }
        public void Add(Partner partner)
        {
            using var dc = new ApplicationContext();
            dc.Partners.Add(partner);
            dc.SaveChanges();
        }

        public List<Partner> GetList()
        {
            using var dc = new ApplicationContext();
            var partners = dc.Partners.ToList();

            return partners;
        }

        public void Edit(int id, Partner oldPartners)
        {
            using var dataContext = new ApplicationContext();
            var newPartners = dataContext.Partners.First(x => x.Id == id);
            newPartners.Name = oldPartners.Name;
            newPartners.UNP = oldPartners.UNP;

            dataContext.SaveChanges();
        }

        public void Delete(Partner partners)
        {
            using var dataContext = new ApplicationContext();
            var newPartners = dataContext.Partners.First(x => x.Id == partners.Id);
            dataContext.Partners.Remove(newPartners);
            dataContext.SaveChanges();
        }

        private static string Cut(string lastName, int n)
        {
            return lastName.Length < n ? lastName : lastName.Substring(0, n);
        }

        public List<Partner> Search(string name, string unp)
        {
            /*var phones = from p in db.Phones
                            where p.CompanyId == 1
                            select p;*/
            //@mark is null or markColumn=@mark
            List<Partner> resultList;
            using var dc = new ApplicationContext();
            {

                //.Select(y => if(y.key.min==0 && y.key.max==0)
                resultList = (from partner in dc.Partners
                    where
name == "" || EF.Functions.Like(partner.Name, '%' + name + '%') &&
(unp == "" || EF.Functions.Like(partner.UNP.Replace(" ", string.Empty), '%' + unp + '%')) 
                    // ((name == "") || EF.Functions.Like(m.Name, name))  &&
                    // ((unity is null ||)) 
                    select partner).ToList();
            }

            return resultList;
            // var resultList = new List<(Partner, int)>();
            Parallel.ForEach(GetList(), (partner) =>
            {
                if (name != string.Empty)
                {
                    int dist = LevenshteinDistance(Cut(partner.Name, name.Length), name);

                }
            });
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
        private static int LevenshteinDistance(string string1, string string2)
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
        }
    }
}
