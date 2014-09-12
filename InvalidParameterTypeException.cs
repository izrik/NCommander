using System;
using System.Collections.Generic;
using System.Linq;

namespace NCommander
{
    public class InvalidParameterTypeException : Exception
    {
        public InvalidParameterTypeException(Parameter parameter)
        {
            if (parameter.Name == null) throw new ArgumentNullException("parameter");

            Parameter = parameter;
        }

        public readonly Parameter Parameter;

        public override string Message
        {
            get
            {
                return string.Format("\"flag\" is not a valid type for a parameter ({0})", Parameter.Name);
            }
        }
    }
}

