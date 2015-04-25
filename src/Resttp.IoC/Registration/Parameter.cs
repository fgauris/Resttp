using System;
using System.Linq.Expressions;

namespace Resttp.IoC.Registration
{
    public class Parameter
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public Parameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

    }
}
