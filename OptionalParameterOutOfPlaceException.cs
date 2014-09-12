using System;
using System.Collections.Generic;
using System.Linq;

namespace NCommander
{
    public class OptionalParameterOutOfPlaceException : Exception
    {
        public OptionalParameterOutOfPlaceException(Parameter parameter)
        {
            if (parameter.Name == null)
                throw new ArgumentNullException("parameter");

            Parameter = parameter;
        }

        public readonly Parameter Parameter;

        public override string Message
        {
            get
            {
                return string.Format("Required parameter \"{0}\" follows an optional parameter.", Parameter.Name);
            }
        }
    }
}

