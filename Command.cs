using System;
using System.Collections.Generic;
using System.Linq;

namespace NCommander
{
    public class Command
    {
        public string Name = string.Empty;
        public string Description = string.Empty;
        public Parameter[] Params = new Parameter[0];

        public string HelpText = string.Empty;
        public Action HelpAction;

        // There are two ways to customize the execution behavior of a command:
        //  1. Assign a delegate to ExecuteDelegate
        //  2. Override InternalExecute in a derived class
        public Action<Dictionary<string, object>> ExecuteDelegate;

        protected virtual void InternalExecute(Dictionary<string, object> args)
        {
            if (ExecuteDelegate != null)
            {
                ExecuteDelegate(args);
            }
        }

        public void Execute(IEnumerable<string> args)
        {
            bool optionalStarted = false;
            foreach (var param in Params)
            {
                if (param.IsOptional)
                {
                    optionalStarted = true;
                }
                else if (optionalStarted)
                {
                    throw new ArgumentException(string.Format("Required parameter \"{0}\" follows an optional parameter.", param.Name), param.Name);
                }
            }

            var convertedArgs = ConvertArguments(args.ToList());

            InternalExecute(convertedArgs);
        }

        protected Dictionary<string, object> ConvertArguments(List<string> args)
        {
            if (args.Count < Params.Length &&
                !Params[args.Count].IsOptional)
            {
                var param = Params[args.Count];
                throw new ArgumentException(string.Format("No value was provided for required parameter \"{0}\".", param.Name), param.Name);
            }

            int i;
            var convertedArgs = new Dictionary<string, object>();
            for (i = 0; i < args.Count; i++)
            {
                if (i >= Params.Length) break;

                var param = Params[i];

                var arg = args[i];
                object value;
                switch (param.ParameterType)
                {
                case ParameterType.Int:
                    int ivalue;
                    if (int.TryParse(arg, out ivalue))
                    {
                        value = ivalue;
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("\"{0}\" is not a valid integer.", arg), param.Name);
                    }
                    break;
                case ParameterType.String:
                    value = arg;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
                }

                convertedArgs.Add(param.Name, value);
            }

            return convertedArgs;
        }
    }
}

