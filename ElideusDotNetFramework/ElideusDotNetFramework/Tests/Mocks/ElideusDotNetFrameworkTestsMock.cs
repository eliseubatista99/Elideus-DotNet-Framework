namespace ElideusDotNetFramework.Tests.Mocks
{
    public class ElideusDotNetFrameworkTestsMock<T>
    {
        protected static readonly object _lock = new object();

        private static T? _instanceMock;

        protected virtual T? ConfigureMock()
        {
            return default;
        }

        public T? Mock()
        {
            lock (_lock)
            {
                if (_instanceMock != null)
                {
                    return _instanceMock;
                }

                _instanceMock = ConfigureMock();
                return _instanceMock;
            }
        }
    }
}
