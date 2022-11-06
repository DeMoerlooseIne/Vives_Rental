using System.Text;

namespace VivesRental.Sdk.Extensions
{
    public static class FilterSdkExtension
    {
        public static string AddQuery(this string route, object? filterObject)
        {
            if (filterObject is null)
            {
                return "";
            }
            var stringBuilder = new StringBuilder($"{route}?");
            var objectType = filterObject.GetType();
            var properties = objectType.GetProperties()
                .Where(p => p.GetValue(filterObject) != null);

            foreach (var property in properties)
            {
                var name = property.Name;
                var value = property.GetValue(filterObject);
                stringBuilder.Append($"{name}={value}&");
            }
            return stringBuilder.ToString().TrimEnd('&');
        }
    }
}
