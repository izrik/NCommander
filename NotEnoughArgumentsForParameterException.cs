using System;
using System.Collections.Generic;
using System.Linq;

namespace NCommander
{
    public class NotEnoughArgumentsForParameterException : Exception
    {
        public NotEnoughArgumentsForParameterException(Parameter parameter)
        {
            if (parameter.Name == null) throw new ArgumentNullException("parameter");

            Parameter = parameter;
        }

        public readonly Parameter Parameter;

        public override string Message
        {
            get
            {
                return string.Format("No value was provided for required parameter \"{0}\".", Parameter.Name);
            }
        }
    }
}

