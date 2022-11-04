using VivesRental.Enums;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Services.Model.Results;

namespace VivesRental.ConsoleApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //await TestGetRentedArticles();
            //await TestGetArticleResults();
            await TestNumberOfAvailableItems();
            //TestIsAvailable();
            //TestRemove();
            //await TestEdit2();
            Console.WriteLine("Done...");
            Console.ReadLine();
        }

        static async Task TestGetRentedArticles()
        {
            await using var context = new VivesRentalDbContext().CreateDbContext();

            var customerService = new CustomerService(context);
            var productService = new ProductService(context);
            var articleService = new ArticleService(context);
            var orderService = new OrderService(context);
            var orderLineService = new OrderLineService(context);

            //Create Customer
            var customer = await customerService.CreateAsync(new Customer { FirstName = "Bavo", LastName = "Ketels", Email = "bavo.ketels@vives.be", PhoneNumber = "test" });
            //Create Product
            var product = await productService.CreateAsync(new Product { Name = "Product", RentalExpiresAfterDays = 1 });
            //Create Article
            var article = await articleService.CreateAsync(new Article { ProductId = product.Id });
            //Rent article
            var order = await orderService.CreateAsync(customer.Id);
            await orderLineService.RentAsync(order.Id, article.Id);

            //Get articles
            var articles = await articleService.GetRentedArticlesAsync();

            ShowArticles(articles);
        }

        static async Task TestGetArticleResults()
        {
            await using var context = new DbContextFactory().CreateDbContext();

            var articleService = new ArticleService(context);

            var articles = await articleService.FindAsync();

            ShowArticles(articles);
        }

        static void ShowArticles(IList<ArticleResult> articles)
        {
            foreach (var article in articles)
            {
                Console.WriteLine($"{article.Id}: {article.ProductName}");
            }
        }

        static async Task TestNumberOfAvailableItems()
        {
            await using var context = new DbContextFactory().CreateDbContext();

            var customerService = new CustomerService(context);
            var productService = new ProductService(context);
            var articleService = new ArticleService(context);
            var articleReservationService = new ArticleReservationService(context);
            var orderService = new OrderService(context);
            var orderLineService = new OrderLineService(context);

            ////Create Customer
            //var customer = await customerService.CreateAsync(new Customer { FirstName = "Bavo", LastName = "Ketels", Email = "bavo.ketels@vives.be", PhoneNumber = "test" });

            ////Create Product
            //var product = await productService.CreateAsync(new Product { Name = "Product", RentalExpiresAfterDays = 1 });

            ////Create Article
            //var article = await articleService.CreateAsync(new Article { ProductId = product.Id });
            //var productResults = await productService.GetAvailableProductResultsAsync();
            //Console.WriteLine($"availableProducts (1): {productResults.First().NumberOfAvailableArticles}");

            ////Edit Article
            //await articleService.UpdateStatusAsync(article.Id, ArticleStatus.Broken);
            //var productResultsBroken = await productService.GetAvailableProductResultsAsync();
            //Console.WriteLine($"availableProducts Broken (0): {productResultsBroken.Count}");

            ////Add OrderLine
            //await articleService.UpdateStatusAsync(article.Id, ArticleStatus.Normal);
            //var order = await orderService.CreateAsync(customer.Id);
            //await orderLineService.RentAsync(order.Id, article.Id);
            //var productResultsRented = await productService.GetAvailableProductResultsAsync();
            //Console.WriteLine($"availableProducts Rented (0): {productResultsRented.Count}");

            ////Return Order
            //await orderService.ReturnAsync(order.Id, DateTime.Now);
            //var productResultsReturned = await productService.GetAvailableProductResultsAsync();
            //Console.WriteLine($"availableProducts Returned (1): {productResultsReturned.First().NumberOfAvailableArticles}");

            ////Create ArticleReservation
            //var articleReservation = await articleReservationService.CreateAsync(customer.Id, article.Id);
            //var productResultsReserved = await productService.GetAvailableProductResultsAsync();
            //Console.WriteLine($"availableProducts Reserved (0): {productResultsReserved.Count}");

            var productIdToTest = new Guid("5475cdd2-256c-4097-aaa5-3ce1327a2cf5");

            var products = await productService.FindAsync();
            foreach (var productItem in products)
            {
                Console.WriteLine($"Product {productItem.Id},  Available Articles: {productItem.NumberOfAvailableArticles}");
            }

            var availableArticlesResult = await articleService.GetAvailableArticlesAsync(productIdToTest);
            Console.WriteLine($"available Articles for Product {productIdToTest}: {availableArticlesResult.Count}");
        }

        static async Task TestIsAvailable()
        {
            await using var context = new DbContextFactory().CreateDbContext();

            var customerService = new CustomerService(context);
            var productService = new ProductService(context);
            var articleService = new ArticleService(context);
            var articleReservationService = new ArticleReservationService(context);
            var orderService = new OrderService(context);
            var orderLineService = new OrderLineService(context);

            //Create Customer
            var customer = await customerService.CreateAsync(new Customer { FirstName = "Bavo", LastName = "Ketels", Email = "bavo.ketels@vives.be", PhoneNumber = "test" });

            //Create Product
            var product = await productService.CreateAsync(new Product { Name = "Product", RentalExpiresAfterDays = 1 });

            //Create Article
            var article = await articleService.CreateAsync(new Article { ProductId = product.Id });
            var isAvailableWithoutReservations = await articleService.IsAvailableAsync(article.Id, DateTime.Now);
            Console.WriteLine($"isAvailableWithoutReservations (true): {isAvailableWithoutReservations}");

            //Edit Article
            await articleService.UpdateStatusAsync(article.Id, ArticleStatus.Broken);
            var isAvailableBroken = await articleService.IsAvailableAsync(article.Id, DateTime.Now);
            Console.WriteLine($"isAvailableBroken (false): {isAvailableBroken}");

            //Add OrderLine
            await articleService.UpdateStatusAsync(article.Id, ArticleStatus.Normal);
            var order = await orderService.CreateAsync(customer.Id);
            await orderLineService.RentAsync(order.Id, article.Id);
            var isAvailableRented = await articleService.IsAvailableAsync(article.Id, DateTime.Now);
            Console.WriteLine($"isAvailableRented (false): {isAvailableRented}");

            //Return Order
            await orderService.ReturnAsync(order.Id, DateTime.Now);
            var isAvailableReturned = await articleService.IsAvailableAsync(article.Id, DateTime.Now);
            Console.WriteLine($"isAvailableReturned (true): {isAvailableReturned}");

            //Create ArticleReservation
            var articleReservation = await articleReservationService.CreateAsync(customer.Id, article.Id);
            var isAvailableWithReservations = await articleService.IsAvailableAsync(article.Id, DateTime.Now);
            Console.WriteLine($"isAvailableWithReservations (false): {isAvailableWithReservations}");
        }

        static async Task TestEdit()
        {
            await using var context = new DbContextFactory().CreateDbContext();

            var productService = new ProductService(context);

            var articleService = new ArticleService(context);

            var product = new Product
            {
                Name = "Test",
                Description = "Test",
                Manufacturer = "Test",
                Publisher = "Test",
                RentalExpiresAfterDays = 10
            };
            var createdProduct = await productService.CreateAsync(product);
            var article = new Article
            {
                ProductId = createdProduct.Id,
                Status = ArticleStatus.Normal
            };
            var createdArticle = await articleService.CreateAsync(article);

            var updateStatusResult = await articleService.UpdateStatusAsync(createdArticle.Id, ArticleStatus.Broken);

            await productService.RemoveAsync(createdProduct.Id);
        }

        static async Task TestEdit2()
        {
            await using var context = new DbContextFactory().CreateDbContext();

            var productService = new ProductService(context);

            var articleService = new ArticleService(context);

            var product = new Product
            {
                Name = "Test",
                Description = "Test",
                Manufacturer = "Test",
                Publisher = "Test",
                RentalExpiresAfterDays = 10
            };
            var createdProduct = await productService.CreateAsync(product);
            var article = new Article
            {
                ProductId = createdProduct.Id,
                Status = ArticleStatus.Normal
            };
            var createdArticle = await articleService.CreateAsync(article);

            var fakeUpdateStatusResult = await articleService.UpdateStatusAsync(Guid.NewGuid(), ArticleStatus.Broken);
            product.Id = Guid.NewGuid();
            var fakeUpdateResult = await productService.EditAsync(product);

            await productService.RemoveAsync(createdProduct.Id);
        }

        static async Task TestRemove()
        {
            using var context = new DbContextFactory().CreateDbContext();

            var productService = new ProductService(context);
            var articleService = new ArticleService(context);
            var customerService = new CustomerService(context);
            var orderService = new OrderService(context);
            var orderLineService = new OrderLineService(context);

            var customer = await customerService.CreateAsync(new Customer
            { FirstName = "Test", LastName = "Test", Email = "test@test.com" });
            var product = await productService.CreateAsync(new Product
            {
                Name = "Test",
                Description = "Test",
                Manufacturer = "Test",
                Publisher = "Test",
                RentalExpiresAfterDays = 10
            });

            var article = await articleService.CreateAsync(new Article
            {
                ProductId = product.Id,
                Status = ArticleStatus.Normal
            });
            var order = await orderService.CreateAsync(customer.Id);
            var orderLine = await orderLineService.RentAsync(order.Id, article.Id);


            var deleteResult = await customerService.RemoveAsync(product.Id);

            var failingDeleteResult = await customerService.RemoveAsync(Guid.NewGuid());
            var failingDeleteResult2 = await productService.RemoveAsync(Guid.NewGuid());
        }

        static async Task TestRemove2()
        {
            await using var context = new DbContextFactory().CreateDbContext();

            var customerService = new CustomerService(context);

            var deleteResult = await customerService.RemoveAsync(Guid.Parse("94EF2D02-FD9D-42AE-6461-08D799014437"));
        }
    }
}