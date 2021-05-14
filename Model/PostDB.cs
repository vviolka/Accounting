using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model.DBStructure;

namespace Model
{
    public class PostDB
    {
        private ApplicationContext ac;
        public PostDB()
        {
            ac = new ApplicationContext();
        }
        private const int LevensteinDist = 2;
        public IEnumerable SearchByName(string name)
        {
            var resultList = new List<(Post, int)>();
            Parallel.ForEach(ac.Posts.ToList(), (post) =>
            {
                int dist = LevenshteinDistance(Cut(post.Name, name.Length), name);
                if (dist >= LevensteinDist) return;
                lock (resultList)
                    resultList.Add((post, dist));
            });
            resultList.Sort((x, y) => -y.Item2.CompareTo(x.Item2));
            return resultList.ConvertAll(input => input.Item1);

        }
        public void Add(Post post)
        {
            using var dc = new ApplicationContext();
            dc.Posts.Add(post);
            dc.SaveChanges();
        }

        public List<Post> GetList()
        {
            using var dc = new ApplicationContext();
            var posts = dc.Posts.ToList();

            return posts;
        }

        public void Edit(int id, Post oldPosts)
        {
            using var dataContext = new ApplicationContext();
            var newPosts = dataContext.Posts.First(x => x.Id == id);
            newPosts.Name = oldPosts.Name;
            newPosts.Rate = oldPosts.Rate;

            dataContext.SaveChanges();
        }

        public void Delete(Post posts)
        {
            using var dataContext = new ApplicationContext();
            var newPosts = dataContext.Posts.First(x => x.Id == posts.Id);
            dataContext.Posts.Remove(newPosts);
            dataContext.SaveChanges();
        }

        private static string Cut(string lastName, int n)
        {
            return lastName.Length < n ? lastName : lastName.Substring(0, n);
        }

        public List<Post> Search(string name, float? rate)
        {
            /*var phones = from p in db.Phones
                            where p.CompanyId == 1
                            select p;*/
            //@mark is null or markColumn=@mark
            List<Post> resultList;
            using var dc = new ApplicationContext();
            {

                //.Select(y => if(y.key.min==0 && y.key.max==0)
                resultList = (from post in dc.Posts
                              where
          name == "" || EF.Functions.Like(post.Name, '%' + name + '%') &&
          (rate == null || post.Rate == rate)
                              // ((name == "") || EF.Functions.Like(m.Name, name))  &&
                              // ((unity is null ||)) 
                              select post).ToList();
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
