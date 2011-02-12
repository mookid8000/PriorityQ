using Norm.BSON;
using Norm.Collections;

namespace Web.Infrastructure
{
    public interface IMongoSession
    {
        IMongoCollection<T> GetCollection<T>();
        void CreateIndex<T>(Expando key, string name, bool unique);
    }
}