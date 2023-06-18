using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestaurantTerminal.Models;

namespace RestaurantTerminal.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Check> Checks { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Finance> Finances { get; set;}
        public DbSet<Person> People { get; set; }
        public DbSet<Role> Roles {  get; set; }
        public DbSet<ActivityStatistics> ActivityStatistics { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDish> OrderDishes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Dishes)
                .WithOne(d => d.Category);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Employee)
                .WithOne(e => e.Person)
                .HasForeignKey<Employee>(e => e.PersonId);
                
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<ActivityStatistics>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.ActivityStatistics)
                .HasForeignKey(a => a.EmployeeId);

            modelBuilder.Entity<Check>()
                .HasOne(c => c.Order)
                .WithOne(o => o.Check)
                .HasForeignKey<Check>(c => c.OrderId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Dishes)
                .WithOne(od => od.Order)
                .HasForeignKey(o => o.OrderId);

            modelBuilder.Entity<OrderDish>()
                .HasOne(od => od.Dish)
                .WithMany(d => d.OrderDishes)
                .HasForeignKey(od => od.DishId);


            base.OnModelCreating(modelBuilder);
        }

    }
}
