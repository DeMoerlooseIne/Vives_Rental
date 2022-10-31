using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Extensions;

namespace VivesRental.Repository.Core;

public class VivesRentalDbContext : DbContext
{
    public VivesRentalDbContext(DbContextOptions options) : base(options)
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

    public void Seed()
    {

        var zoëCustomer = new Customer { FirstName = "Odin", LastName = "Depreeuw", Email = "kimmetje@hotmail.com", PhoneNumber = "0475/53.26.45" };
        var odinCustomer = new Customer { FirstName = "Zoë", LastName = "Depreeuw", Email = "kimmetje@hotmail.com", PhoneNumber = "0475/53.26.45" };
        var kimCustomer = new Customer { FirstName = "Kim", LastName = "De Moerloose", Email = "kimmetje@hotmail.com", PhoneNumber = "0475/53.26.45"};
        var ineCustomer = new Customer { FirstName = "Ine", LastName = "De Moerloose", Email = "enitjeun@hotmail.com", PhoneNumber = "0475/53.26.45" };


        Customers.Add(zoëCustomer);
        Customers.Add(odinCustomer);
        Customers.Add(ineCustomer);
        Customers.Add(kimCustomer);

        var firstProduct = new Product
        {
            Name = "Coca cola",
            Description = "TMI",
            Manufacturer = "Coca Cola Company",
            Publisher = "Coca Cola Company",
            RentalExpiresAfterDays = 30
        };

        var secondProduct = new Product
        {
            Name = "Coca cola",
            Description = "TMI",
            Manufacturer = "Coca Cola Company",
            Publisher = "Coca Cola Company",
            RentalExpiresAfterDays = 30
        };

        var thirdProduct = new Product
        {
            Name = "Coca cola",
            Description = "TMI",
            Manufacturer = "Coca Cola Company",
            Publisher = "Coca Cola Company",
            RentalExpiresAfterDays = 30
        };

        var forthProduct = new Product
        {
            Name = "Coca cola",
            Description = "TMI",
            Manufacturer = "Coca Cola Company",
            Publisher = "Coca Cola Company",
            RentalExpiresAfterDays = 30
        };

        Products.Add(firstProduct);
        Products.Add(secondProduct);
        Products.Add(thirdProduct);
        Products.Add(forthProduct);

        var firstArticle = new Article
        {
            ProductId = Guid.Empty,
            Status = Enums.ArticleStatus.Normal
        };

        var secondArticle = new Article
        {
            ProductId = Guid.Empty,
            Product = new Product
            {
                Id = Guid.Empty,
                Name = "Coca Cola"
            },
            Status = Enums.ArticleStatus.Normal
        };


        var firstArticleReservation = new ArticleReservation
        {
            ArticleId = Guid.Empty,
            CustomerId = Guid.Empty,
            FromDateTime = DateTime.Now,
            UntilDateTime = DateTime.UtcNow.AddDays(15)
        };

        var secondArticleReservation = new ArticleReservation
        {
            ArticleId = Guid.Empty,
            CustomerId = Guid.Empty,
            FromDateTime = DateTime.Now,
            UntilDateTime = DateTime.UtcNow.AddDays(15)
        };


        SaveChanges();
    }
}