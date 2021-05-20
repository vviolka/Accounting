using Microsoft.EntityFrameworkCore;
using Model.DBStructure;

namespace Model
{
    public sealed class ApplicationContext : DbContext
    {

        public DbSet<Balance> Balances { get; set; }
        public DbSet<BillOfLading> BillsOfLading { get; set; }
      //  public DbSet<Child> Children  { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeePost> EmployeesPosts  { get; set; }
        public DbSet<Expence> Expences { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<MaterialsInfo> MaterialInfoes { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Production> Productions { get; set; }
        
        public DbSet<PartnersBalances> PartnersBalances { get; set; }
       // public DbSet<Salary> Salaries { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(@"User Id=ADMIN;Password=accounting_database_Wikentia10;Data Source=accounting_high");
        }

    }
}