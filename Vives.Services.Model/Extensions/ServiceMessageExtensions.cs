namespace Vives.Services.Model.Extensions
{
    public static class ServiceMessageExtensions
    {
        public static T JsonNull<T>(this T serviceResult)
            where T: ServiceResult
            serviceResult.Messages.Add(new ServiceMessage
            {
                Code = "JsonNullError",
                Message = "Could not parse json.",
                Type = ServiceMessageType.Error
            });
            return serviceResult;
        }

        public static T NotFound<T>(this T serviceResult, string entityName)
            where T:ServiceResult
        {
            serviceResult.Messages.Add(new ServiceMessage
            {
                Code = "NotFound",
                Message = $"Could not find {entityName}.",
                Type = ServiceMessageType.Error
            });
            return serviceResult;
        }

        public static T Unauthorized<T>(this T serviceResult)
            where T: ServiceResult
        {
            serviceResult.Messages.Add(new ServiceMessage
            {
                Code = "Unauthorized",
                Message = "You are not authorized.",
                Type = ServiceMessageType.Error
            });
            return serviceResult;
        }
    }
}
