using NSubstituteAutoMocker;

namespace PkoAnalizer.Tests
{
    public class BaseUnitTest<SUT>
        where SUT : class
    {
        private readonly NSubstituteAutoMocker<SUT> mock = new NSubstituteAutoMocker<SUT>();

        public SUT Sut => mock.ClassUnderTest;

        public T Mock<T>() where T : class
        {
            return mock.Get<T>();
        }
    }
}
