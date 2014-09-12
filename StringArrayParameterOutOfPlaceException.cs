using System;
using System.Collections.Generic;
using System.Linq;

namespace NCommander
{
    public class StringArrayParameterOutOfPlaceException : NCommanderException
    {
        public StringArrayParameterOutOfPlaceException(Parameter parameter)
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
                return string.Format("The parameter \"{0}\" has a type of string array but is not the last parameter in the list.", Parameter.Name);
            }
        }
    }
}

