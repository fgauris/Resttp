using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp.Dependencies
{
    public interface IDependencyResolver: IDisposable
    {
        object Resolve(Type type);
    }
}
