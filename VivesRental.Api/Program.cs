using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using VivesRental.Repository.Core;
using VivesRental.Services;
using VivesRental.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(x =>
{
    //handy tool working with REST-API Swagger :
    // Serializes enums as strings in api responses 
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

builder.Services.AddDbContext<VivesRentalDbContext>(options =>
{
    options.UseInMemoryDatabase(nameof(VivesRentalDbContext));
});

builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IArticleReservationService, ArticleReservationService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderLineService, OrderLineService>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<VivesRentalDbContext>();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
