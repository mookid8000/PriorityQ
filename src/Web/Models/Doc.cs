using MongoDB.Bson;

namespace Web.Models
{
    public abstract class Doc
    {
        public static ObjectId NewId()
        {
            return FakeDoc.NewId();
        }
    }
}