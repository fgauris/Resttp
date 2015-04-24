using System;
using System.Linq.Expressions;

namespace Resttp.IoC.Registration
{
    public class Parameter
    {
        public Type ArgType { get; private set; }
        public string Name { get; private set; }
        public ConstantExpression Value { get; private set; }

        public Parameter(Type type, string name, ConstantExpression value)
        {
            ArgType = type;
            Name = name;
            Value = value;
        }

    }
}
