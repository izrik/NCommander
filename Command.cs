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
        public Option[] Options = new Option[0];

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
                if (param.ParameterType == ParameterType.Flag)
                {
                    throw new InvalidParameterTypeException(param);
                }

                if (param.IsOptional)
                {
                    optionalStarted = true;
                }
                else if (optionalStarted)
                {
                    throw new OptionalParameterOutOfPlaceException(param);
                }

                if (param.ParameterType == ParameterType.StringArray &&
                    Array.IndexOf(Params, param) != Params.Length - 1)
                {
                    throw new StringArrayParameterOutOfPlaceException(param);
                }
            }


            var convertedArgs = ConvertArguments(args.ToList());

            InternalExecute(convertedArgs);
        }

        protected Dictionary<string, object> ConvertArguments(List<string> args)
        {
            var convertedArgs = new Dictionary<string, object>();

            foreach (var option in Options)
            {
                if (option.Type == ParameterType.Flag)
                {
                    convertedArgs[option.Name] = false;
                }
                else
                {
                    convertedArgs[option.Name] = null;
                }
            }

            int i;
            int p = 0;
            var stringArray = new List<string>();
            for (i = 0; i < args.Count; i++)
            {
                var arg = args[i];

                if (arg.StartsWith("--"))
                {
                    bool found = false;
                    foreach (var option in Options)
                    {
                        var longName = "--" + option.Name;
                        if (arg == longName)
                        {
                            found = true;
                            if (option.Type == ParameterType.Flag)
                            {
                                convertedArgs[option.Name] = true;
                            }
                            else
                            {
                                if (i + 1 >= args.Count)
                                {
                                    throw new NotEnoughArgumentsForOptionException(option);
                                }
                                i++;

                                object value = option.Type.ConvertAction(args[i]);
                                convertedArgs[option.Name] = value;
                            }
                            break;
                        }
                    }

                    if (!found)
                    {
                        throw new UnrecognizedOptionException(arg);
                    }
                }
                else
                {
                    if (p >= Params.Length) continue;

                    var param = Params[p];

                    object value;
                    if (param.ParameterType == ParameterType.StringArray)
                    {
                        stringArray.Add(arg);
                    }
                    else
                    {
                        p++;
                        value = param.ParameterType.Convert(arg);
                        convertedArgs.Add(param.Name, value);
                    }
                }
            }

            if (p < Params.Length &&
                Params[p].ParameterType == ParameterType.StringArray)
            {
                var param = Params[p];
                convertedArgs[param.Name] = stringArray.ToArray();
                p++;
            }

            if (p < Params.Length &&
                !Params[p].IsOptional)
            {
                throw new NotEnoughArgumentsForParameterException(Params[p]);
            }

            return convertedArgs;
        }
    }
}

