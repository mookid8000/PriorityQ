using System.Collections;
using System.Linq;
using MongoDB.Bson;

namespace Web.Extensions
{
    public static class StringExtensions
    {
        public static ObjectId AsObjectId(this string str)
        {
            try
            {
                return new ObjectId(str);
            }
            catch
            {
                return new ObjectId();
            }
        }

        public static string JoinToString(this IEnumerable enumerable, string separator)
        {
            return string.Join(separator, enumerable.Cast<object>()
                                              .Select(i => i == null ? "(null)" : i.ToString())
                                              .ToArray());
        }
    }
}