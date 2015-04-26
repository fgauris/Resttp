using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resttp.Dependencies
{
    public interface IDependencyResolver: IDisposable
    {
        T Resolve<T>();
        //object Resolve(Type type);
    }
}
