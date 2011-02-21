using System;
using MongoDB.Bson;

namespace Web.Models
{
    public class FakeDoc
    {
        public static Func<ObjectId> NewId = ObjectId.GenerateNewId;

        public static void Reset()
        {
            NewId = ObjectId.GenerateNewId;
        }
    }
}