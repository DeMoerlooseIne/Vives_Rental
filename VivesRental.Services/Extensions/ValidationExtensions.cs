using Vives.Services.Model;
using Vives.Services.Model.Extensions;
using VivesRental.Model;

namespace VivesRental.Services.Extensions;
public static class ValidationExtensions
{ 
    public class ValidationResult : ServiceResult
    {
    }

    public static ValidationResult IsValid(this Product product)
    {
        ValidationResult validMessage = new();

        if (product == null)
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "DataIsNull",
                Message = "The product is null.",
                Type = ServiceMessageType.Error
            });
        }
        else
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                validMessage.Messages.Add(new ServiceMessage
                {
                    Code = "NameIsNull",
                    Message = "The product name is missing or just whitespace.",
                    Type = ServiceMessageType.Error
                });
            }
        }
        return validMessage;
    }

    public static ValidationResult IsValid(this Article article)
    {
        ValidationResult validMessage = new();

        if (article == null)
        {
            validMessage.DataIsNull("article");
        }
        else
        {
            if (article.ProductId == Guid.Empty)
            {
                validMessage.InvalidId("productid");
            }
        }
        return validMessage;
    }

    public static ValidationResult IsValid(this Order order)
    {
        ValidationResult validMessage = new();

        if (order.CustomerId == Guid.Empty)
        {
            validMessage.InvalidId("customerid");
        }

        if (string.IsNullOrWhiteSpace(order.CustomerFirstName))
        {
            validMessage.DataIsNull("customer's firstname");
        }

        if (string.IsNullOrWhiteSpace(order.CustomerLastName))
        {
            validMessage.DataIsNull("customer's lastname");
        }

        if (string.IsNullOrWhiteSpace(order.CustomerEmail))
        {
            validMessage.DataIsNull("customer's email");
        }

        if (order.CreatedAt == DateTime.MinValue)
        {
            validMessage.InvalidDate();
        }
        return validMessage;
    }

    public static ValidationResult IsValid(this OrderLine orderLine)
    {
        ValidationResult validMessage = new();

        if (orderLine.OrderId == Guid.Empty)
        {
            validMessage.InvalidId("orderid");
        }

        if (orderLine.ArticleId == Guid.Empty)
        {
            validMessage.InvalidId("articleid");
        }

        if (string.IsNullOrWhiteSpace(orderLine.ProductName))
        {
            validMessage.DataIsNull("productname");
        }

        if (orderLine.RentedAt == DateTime.MinValue)
        {
            validMessage.InvalidDate();
        }

        if (orderLine.ExpiresAt == DateTime.MinValue)
        {
            validMessage.InvalidDate();
        }
        return validMessage;
    }

    public static ValidationResult IsValid(this Customer customer)
    {
        ValidationResult validMessage = new();

        if (string.IsNullOrWhiteSpace(customer.FirstName))
        {
            validMessage.DataIsNull("customer's firstname");
        }

        if (string.IsNullOrWhiteSpace(customer.LastName))
        {
            validMessage.DataIsNull("customer's lastname");
        }

        if (string.IsNullOrWhiteSpace(customer.Email))
        {
            validMessage.DataIsNull("customer's email");
        }
        return validMessage;
    }

    public static ValidationResult IsValid(this ArticleReservation articleReservation)
    {
        ValidationResult validMessage = new();

        if (articleReservation.ArticleId == Guid.Empty)
        {
            validMessage.InvalidId("articleid");
        }

        if (articleReservation.CustomerId == Guid.Empty)
        {
            validMessage.InvalidId("customerid");
        }

        //Do not allow an Until date before From date
        if (articleReservation.UntilDateTime < articleReservation.FromDateTime)
        {
            validMessage.InvalidDate();
        }
        return validMessage;
    }
}