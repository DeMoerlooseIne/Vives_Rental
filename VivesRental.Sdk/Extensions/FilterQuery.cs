using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivesRental.Sdk.Extensions
{
    public static class FilterQuery
    {
        public static string AddQuery(this string route, object? filterObject)
        {
            if (filterObject is null)
            {
                return "";
            }
            var sb = new StringBuilder($"{route}?");

            var objType = filterObject.GetType();

            var properties = objType.GetProperties()
                .Where(p => p.GetValue(filterObject) != null);

            foreach (var prop in properties)
            {
                var name = prop.Name;
                var value = prop.GetValue(filterObject);
                sb.Append($"{name}={value}&");
            }
            return sb.ToString().TrimEnd('&');
        }
    }
}
