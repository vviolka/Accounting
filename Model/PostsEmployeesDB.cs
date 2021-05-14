using System.Collections;
using System.Linq;
using Model.DBStructure;

namespace Model
{
    public class PostsEmployeesDB
    {
        public PostsEmployeesDB()
        {
            
        }

        public void Add(EmployeePost employeePost)
        {
            using var ac = new ApplicationContext();
            ac.EmployeesPosts.Add(employeePost);
            ac.SaveChanges();
        }

        public void Edit(EmployeePost oldEmployeePost, EmployeePost newEmployeePost)
        {
            using var ac = new ApplicationContext();
            oldEmployeePost.EmployeeId = newEmployeePost.EmployeeId;
            oldEmployeePost.PostId = newEmployeePost.PostId;
            oldEmployeePost.Bid = newEmployeePost.Bid;
        }

        public void Delete(EmployeePost employeePost)
        {
            using var ac = new ApplicationContext();
            ac.EmployeesPosts.Remove(employeePost);
        }

        public IEnumerable GetAdaptedList()
        {
            using var ac = new ApplicationContext();
            var result = from employeePost in ac.EmployeesPosts
                join employee in ac.Employees on employeePost.EmployeeId equals employee.Id
                join post in ac.Posts on employeePost.PostId equals post.Id
                select new
                {
                    Employee = employee.LastName + "\0" + employee.Name + "\0" + employee.MiddleName,
                    Post = post.Name,
                    Bid = employeePost.Bid
                };
            return result.ToList();

        }
    }
}
