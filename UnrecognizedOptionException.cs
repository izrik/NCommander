using System;
using System.Collections.Generic;
using System.Linq;

namespace NCommander
{
    class UnrecognizedOptionException : Exception
    {
        public UnrecognizedOptionException(string arg)
        {
            if (arg == null) throw new ArgumentNullException("arg");

            this.Argument = arg;
        }

        public readonly string Argument;

        public override string Message
        {
            get
            {
                return string.Format("Unrecognized option: {0}", Argument);
            }
        }
    }

}

