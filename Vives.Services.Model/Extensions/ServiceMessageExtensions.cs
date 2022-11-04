namespace Vives.Services.Model.Extensions
{
    public static class ServiceMessageExtensions
    {
        public static T NoChanges<T>(this T serviceResult)
                   where T : ServiceResult
        {
            serviceResult.Messages.Add(new ServiceMessage
            {
                Code = "NoChanges",
                Message = "Nothing was changed.",
                Type = ServiceMessageType.Info
            });
            return serviceResult;
        }

        public static T ErrorNoChanges<T>(this T serviceResult)
           where T : ServiceResult
        {
            serviceResult.Messages.Add(new ServiceMessage()
            {
                Code = "NoChanges",
                Message = "No changes were made, please try again.",
                Type = ServiceMessageType.Error
            });
            return serviceResult;
        }

        public static T NotFound<T>(this T serviceResult, string entityName)
            where T : ServiceResult
        {
            serviceResult.Messages.Add(new ServiceMessage
            {
                Code = "NotFound",
                Message = $"Could not find {entityName}.",
                Type = ServiceMessageType.Error
            });
            return serviceResult;
        }

        public static T SuccesfullyAdded<T>(this T serviceResult, string entityName)
            where T : ServiceResult
        {
            serviceResult.Messages.Add(new ServiceMessage
            {
                Code = "SuccesfullyAdded",
                Message = $"The {entityName} was succesfully added.",
                Type = ServiceMessageType.Info
            });
            return serviceResult;
        }

        public static T SuccesfullyDeleted<T>(this T serviceResult, string entityName)
            where T : ServiceResult
        {
            serviceResult.Messages.Add(new ServiceMessage
            {
                Code = "SuccesfullyDeleted",
                Message = $"The {entityName} was succesfully deleted.",
                Type = ServiceMessageType.Error
            });
            return serviceResult;
        }
        public static T SuccesfullyReturned<T>(this T serviceResult, string entityName)
            where T : ServiceResult
        {
            serviceResult.Messages.Add(new ServiceMessage
            {
                Code = "SuccesfullyReturned",
                Message = $"The {entityName} was succesfully returned.",
                Type = ServiceMessageType.Error
            });
            return serviceResult;
        }

        public static T WrongId<T>(this T serviceResult)
            where T : ServiceResult
        {
            serviceResult.Messages.Add(new ServiceMessage()
            {
                Code = "WrongId",
                Message = "Please give a correct id.",
                Type = ServiceMessageType.Error
            });
            return serviceResult;
        }

        public static T DataIsNull<T>(this T serviceResult)
            where T : ServiceResult
        {
            serviceResult.Messages.Add(new ServiceMessage()
            {
                Code = "NullData",
                Message = "Data is null.",
                Type = ServiceMessageType.Error
            });
            return serviceResult;
        }

        public static T WrongDate<T>(this T serviceResult)
            where T : ServiceResult
        {
            serviceResult.Messages.Add(new ServiceMessage()
            {
                Code = "WrongDate",
                Message = "Please give a correct date.",
                Type = ServiceMessageType.Error
            });
            return serviceResult;
        }

        public static T ArticleAlreadyReturned<T>(this T serviceResult)
                where T : ServiceResult
        {
            serviceResult.Messages.Add(new ServiceMessage()
            {
                Code = "ArticleAlreadyReturned",
                Message = "This article was already returned.",
                Type = ServiceMessageType.Error
            });
            return serviceResult;
        }
    }
}
