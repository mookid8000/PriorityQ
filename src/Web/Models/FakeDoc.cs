using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Web.Models
{
    public class FakeDoc
    {
        static readonly Queue<ObjectId> StubbedIds = new Queue<ObjectId>();

        static Func<ObjectId> newIdFactoryMethod = ObjectId.GenerateNewId;

        public static ObjectId NewId()
        {
            return newIdFactoryMethod();
        }

        public static void Reset()
        {
            StubbedIds.Clear();

            newIdFactoryMethod = ObjectId.GenerateNewId;
        }

        public static void StubId(ObjectId id)
        {
            StubbedIds.Enqueue(id);

            newIdFactoryMethod = () => StubbedIds.Count > 0
                              ? StubbedIds.Dequeue()
                              : ObjectId.GenerateNewId();
        }
    }
}