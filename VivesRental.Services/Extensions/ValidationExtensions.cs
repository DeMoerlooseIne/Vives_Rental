using VivesRental.Model;
using Vives.Services.Model;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services.Extensions;

public static class ValidationExtensions
{
    public class ValidationMessage
    {
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }
    }

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
        ValidationResult validationMessage = new();


        if (article.ProductId == Guid.Empty)
        {
            validationMessage.Messages.Add(new ServiceMessage
            {
                Code = "InvalidGUID",
                Message = "The article product ID is invalid.",
                Type = ServiceMessageType.Error
            });
        }

        return validationMessage;
    }

    public static ValidationMessage IsValid(this Order order)
    {
        ValidationMessage validationMessage = new ValidationMessage() { IsValid = true };

        if (order.CustomerId == Guid.Empty)
        {
            validationMessage.ErrorMessage = "Please fill in a customerid. The customerid is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }

        if (string.IsNullOrWhiteSpace(order.CustomerFirstName))
        {
            validationMessage.ErrorMessage = "Please fill in a the firstname of the customer. The firstname is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }

        if (string.IsNullOrWhiteSpace(order.CustomerLastName))
        {
            validationMessage.ErrorMessage = "Please fill in a the lastname of the customer. The lastname is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }

        if (string.IsNullOrWhiteSpace(order.CustomerEmail))
        {
            validationMessage.ErrorMessage = "Please fill in a customeremail. The customeremail is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }

        if (order.CreatedAt == DateTime.MinValue)
        {
            validationMessage.ErrorMessage = "Please fill in a correct date. The date is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }
        return validationMessage;
    }

    public static ValidationMessage IsValid(this OrderLine orderLine)
    {
        ValidationMessage validationMessage = new ValidationMessage() { IsValid = true };

        if (orderLine.OrderId == Guid.Empty)
        {
            validationMessage.ErrorMessage = "Please fill in the correct orderid. The orderid is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }

        if (orderLine.ArticleId == Guid.Empty)
        {
            validationMessage.ErrorMessage = "Please fill in the correct articleid. The articleid is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }

        if (string.IsNullOrWhiteSpace(orderLine.ProductName))
        {
            validationMessage.ErrorMessage = "Please fill in a correct productname. The productname is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }

        if (orderLine.RentedAt == DateTime.MinValue)
        {
            validationMessage.ErrorMessage = "Please fill in a correct date. The date is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }

        if (orderLine.ExpiresAt == DateTime.MinValue)
        {
            validationMessage.ErrorMessage = "Please fill in a correct date. The date is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }
        return validationMessage;
    }

    public static ValidationMessage IsValid(this Customer customer)
    {
        ValidationMessage validationMessage = new ValidationMessage() { IsValid = true };

        if (string.IsNullOrWhiteSpace(customer.FirstName))
        {
            validationMessage.ErrorMessage = "Please fill in a correct firstname. The firstname is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }

        if (string.IsNullOrWhiteSpace(customer.LastName))
        {
            validationMessage.ErrorMessage = "Please fill in a correct lastname. The lastname is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }

        if (string.IsNullOrWhiteSpace(customer.Email))
        {
            validationMessage.ErrorMessage = "Please fill in a correct email. The customeremail is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }
        return validationMessage;
    }

    public static ValidationMessage IsValid(this ArticleReservation articleReservation)
    {
        ValidationMessage validationMessage = new ValidationMessage() { IsValid = true };

        if (articleReservation.ArticleId == Guid.Empty)
        {
            validationMessage.ErrorMessage = "Please fill in the correct articleid. The articleid is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }

        if (articleReservation.CustomerId == Guid.Empty)
        {
            validationMessage.ErrorMessage = "Please fill in the correct customerid. The customerid is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }

        //Do not allow an Until date before From date
        if (articleReservation.UntilDateTime < articleReservation.FromDateTime)
        {
            validationMessage.ErrorMessage = "Please fill in a correct date. The date is invalid.";
            validationMessage.IsValid = false;
            return validationMessage;
        }
        return validationMessage;
    }
}