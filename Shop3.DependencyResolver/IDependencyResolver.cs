using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.DependencyResolver
{
    public interface IDependencyResolver
    {
        void SetUp(IDependencyRegister dependencyRegister);

    }
}
