using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Extensions;

namespace VivesRental.Repository.Core;

public class VivesRentalDbContext: DbContext
{
    public VivesRentalDbContext(DbContextOptions options): base(options)
    {
            
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<ArticleReservation> ArticleReservations => Set<ArticleReservation>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RemovePluralizingTableNameConvention();
        base.OnModelCreating(modelBuilder);
    }

    //public void Seed()
    //{

    //    var bavoEmployee = new EmployeeResult { FirstName = "Bavo", LastName = "Ketels" };
    //    var johnEmployee = new EmployeeResult { FirstName = "John", LastName = "Doe" };

    //    Employees.Add(bavoEmployee);
    //    Employees.Add(johnEmployee);

    //    var firstTicket = new HelpDeskTicketResult
    //    {
    //        Id = 1,
    //        Issue = "First article ticket",
    //        Description = "Short description of first ticket",
    //        Status = "Handled",
    //        EmployeeId = bavoEmployee.Id,
    //        TicketHandler = bavoEmployee,
    //        CreatedDate = DateTime.Now
    //    };

    //    var secondTicket = new HelpDeskTicketResult
    //    {
    //        Id = 2,
    //        Issue = "Second article ticket",
    //        Description = "Short description of second ticket",
    //        Status = "In progress",
    //        EmployeeId = johnEmployee.Id,
    //        CreatedDate = DateTime.Now
    //    };

    //    HelpDeskTickets.Add(firstTicket);
    //    HelpDeskTickets.Add(secondTicket);

    //    SaveChanges();
    //}
}