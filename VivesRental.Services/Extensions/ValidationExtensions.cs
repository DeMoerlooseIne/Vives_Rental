using Vives.Services.Model;
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
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "DataIsNull",
                Message = "The article is null.",
                Type = ServiceMessageType.Error
            });
        }
        else
        {
            if (article.ProductId == Guid.Empty)
            {
                validMessage.Messages.Add(new ServiceMessage
                {
                    Code = "InvalidId",
                    Message = "The productid of the article is invalid.",
                    Type = ServiceMessageType.Error
                });
            }
        }
        return validMessage;
    }

    public static ValidationResult IsValid(this Order order)
    {
        ValidationResult validMessage = new();

        if (order.CustomerId == Guid.Empty)
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "InvalidId",
                Message = "The customerid is invalid or empty.",
                Type = ServiceMessageType.Error
            });
        }

        if (string.IsNullOrWhiteSpace(order.CustomerFirstName))
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "FirstNameNull",
                Message = "Firstname is null or empty.",
                Type = ServiceMessageType.Error
            });
        }

        if (string.IsNullOrWhiteSpace(order.CustomerLastName))
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "LastNameNull",
                Message = "Firstname is null or empty.",
                Type = ServiceMessageType.Error
            });
        }

        if (string.IsNullOrWhiteSpace(order.CustomerEmail))
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "EmailNull",
                Message = "Email is null or empty.",
                Type = ServiceMessageType.Error
            });
        }

        if (order.CreatedAt == DateTime.MinValue)
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "InvalidDate",
                Message = "Date is invalid.",
                Type = ServiceMessageType.Error
            });
        }
    
        return validMessage;
    }

    public static ValidationResult IsValid(this OrderLine orderLine)
    {
        ValidationResult validMessage = new();

        if (orderLine.OrderId == Guid.Empty)
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "InvalidId",
                Message = "Orderid is invalid or empty.",
                Type = ServiceMessageType.Error
            });
        }

        if (orderLine.ArticleId == Guid.Empty)
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "InvalidId",
                Message = "Articleid is invalid or empty.",
                Type = ServiceMessageType.Error
            });
        }

        if (string.IsNullOrWhiteSpace(orderLine.ProductName))
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "ProductNameNull",
                Message = "Productname is null or empty.",
                Type = ServiceMessageType.Error
            });
        }

        if (orderLine.RentedAt == DateTime.MinValue)
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "InvalidDate",
                Message = "Date is invalid.",
                Type = ServiceMessageType.Error
            });
        }

        if (orderLine.ExpiresAt == DateTime.MinValue)
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "InvalidDate",
                Message = "Date is invalid.",
                Type = ServiceMessageType.Error
            });
        }
        return validMessage;
    }

    public static ValidationResult IsValid(this Customer customer)
    {
        ValidationResult validMessage = new();

        if (string.IsNullOrWhiteSpace(customer.FirstName))
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "FirstNameNull",
                Message = "Firstname is null or empty.",
                Type = ServiceMessageType.Error
            });
        }

        if (string.IsNullOrWhiteSpace(customer.LastName))
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "LastNameNull",
                Message = "Lastname is null or empty.",
                Type = ServiceMessageType.Error
            });
        }

        if (string.IsNullOrWhiteSpace(customer.Email))
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "EmailNull",
                Message = "Email is null or empty.",
                Type = ServiceMessageType.Error
            });
        }
        return validMessage;
    }

    public static ValidationResult IsValid(this ArticleReservation articleReservation)
    {
        ValidationResult validMessage = new();

        if (articleReservation.ArticleId == Guid.Empty)
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "InvalidId",
                Message = "Articleid is invalid or empty.",
                Type = ServiceMessageType.Error
            });
        }

        if (articleReservation.CustomerId == Guid.Empty)
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "InvalidId",
                Message = "Customerid is invalid or empty.",
                Type = ServiceMessageType.Error
            });
   

        //Do not allow an Until date before From date
        if (articleReservation.UntilDateTime < articleReservation.FromDateTime)
        {
            validMessage.Messages.Add(new ServiceMessage
            {
                Code = "InvalidDate",
                Message = "Date is invalid.",
                Type = ServiceMessageType.Error
            });
        }
        return validMessage;
    }
}