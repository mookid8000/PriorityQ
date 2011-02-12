using Rhino.Mocks;

namespace Test
{
    public abstract class FixtureBase
    {
        protected T Mock<T>() where T:class
        {
            return MockRepository.GenerateMock<T>();
        }
    }
}