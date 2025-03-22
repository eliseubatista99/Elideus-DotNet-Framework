using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElideusDotNetFramework.Tests
{
    public class ElideusDotNetFrameworkTestsBuilder
    {
        protected static bool _initialized = false;
        protected static readonly object _lock = new object();

        protected virtual void ConfigureMocks()
        {
           
        }

        public void InitializeTests()
        {
            lock (_lock)
            {
                if (_initialized)
                {
                    return;
                }

                ConfigureMocks();

                _initialized = true;
            }
        }
    }
}
