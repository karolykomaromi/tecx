using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TecX.Unity.Configuration.Conventions
{
    public interface IRegistrationConvention
    {
        void Process(Type type, Registry registry);
    }
}
