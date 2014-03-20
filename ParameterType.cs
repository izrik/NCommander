using System;

namespace NCommander
{
    public class ParameterType
    {
        public static readonly ParameterType Integer = new ParameterType(
                                                   name: "integer",
                                                   description: "A whole number, positive or negative",
                                                   helpText: string.Format("Any integer from {0} to {1}", int.MinValue, int.MaxValue),
                                                   outputType: typeof(int),
                                                   convertAction: x => int.Parse(x)
                                               );

        public static readonly ParameterType String = new ParameterType(
                                                  name: "string",
                                                  description: "A string; no conversion is performed",
                                                  // helpText: "A string; no conversion is performed",
                                                  outputType: typeof(string),
                                                  convertAction: (x) => x
                                              );

        public ParameterType(string name, Func<string, object> convertAction, string description="", string helpText="", Type outputType=null)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (convertAction == null) throw new ArgumentNullException("convertAction");
            if (description == null) description = "";
            if (helpText == null) helpText = "";
            if (outputType == null) outputType = typeof(object);

            Name = name;
            ConvertAction = convertAction;
            Description = description;
            HelpText = helpText;
            OutputType = outputType;
        }

        public readonly string Name;
        public Func<string, object> ConvertAction;
        public readonly string Description;
        public readonly string HelpText;
        public readonly Type OutputType;

        public virtual object Convert(string arg)
        {
            if (ConvertAction == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "No conversion defined for parameter type \"{0}\"", 
                        Name));
            }

            var convertedValue = ConvertAction(arg);

            var convertedType = convertedValue.GetType();
                if (!OutputType.IsAssignableFrom(convertedType))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "The converted value of \"{0}\" should be {1}, but is {2}",
                        arg,
                        OutputType,
                        convertedType));
            }

            return convertedValue;
        }
    }
}

