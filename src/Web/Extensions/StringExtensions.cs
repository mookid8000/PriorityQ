using Norm;

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
    }
}