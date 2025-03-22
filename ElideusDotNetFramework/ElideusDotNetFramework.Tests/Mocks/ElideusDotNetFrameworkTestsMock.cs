using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElideusDotNetFramework.Tests.Mocks
{
    public class ElideusDotNetFrameworkTestsMock<T>
    {
        protected static readonly object _lock = new object();

        private static T _instanceMock;

        protected virtual T ConfigureMock()
        {
            return default(T);
        }

        public T Mock()
        {
            lock (_lock)
            {
                if (_instanceMock != null)
                {
                    return _instanceMock;
                }

                return ConfigureMock();
            }
        }
    }
}
