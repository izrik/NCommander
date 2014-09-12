using System;
using System.Collections.Generic;
using System.Linq;

namespace NCommander
{
    public class NotEnoughArgumentsForOptionException : Exception
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
                return string.Format("Ran out of arguments (option \"{0}\")", Option.Name);
            }
        }
    }


}

