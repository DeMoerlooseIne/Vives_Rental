//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;
//using VivesRental.Repository.Core;

//namespace VivesRental.ConsoleApp.Factories
//{
//    public class DbContextFactory : IDesignTimeDbContextFactory<VivesRentalDbContext>
//    {
//        private static string _connectionString;

//        public VivesRentalDbContext CreateDbContext()
//        {
//            return CreateDbContext(null);
//        }

//        public VivesRentalDbContext CreateDbContext(string[] args)
//        {
//            if (string.IsNullOrEmpty(_connectionString))
//            {
//                LoadConnectionString();
//            }

//            var builder = new DbContextOptionsBuilder<VivesRentalDbContext>();
//            builder.UseSqlServer(_connectionString);

//            return new VivesRentalDbContext(builder.Options);
//        }

//        private static void LoadConnectionString()
//        {
//            var builder = new ConfigurationBuilder();
//            builder.AddJsonFile("appsettings.json", optional: false);

//            var configuration = builder.Build();

//            _connectionString = configuration.GetConnectionString("VivesRentalDbContext");
//        }
//    }
//}
