using System;
using System.Collections.Generic;
using System.Linq;

namespace NCommander
{
    public class NotEnoughArgumentsForOptionException : NCommanderException
    {
        public NotEnoughArgumentsForOptionException(Option option)
        {
            if (option == null) throw new ArgumentNullException("option");

            Option = option;
        }

        public readonly Option Option;

        public override string Message
        {
            get
            {
                return string.Format("No value was provided for option \"{0}\"", Option.Name);
            }
        }
    }


}

