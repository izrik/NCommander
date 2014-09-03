using System;

namespace NCommander
{
    public class Option
    {
        public string Name;
        public string Description = string.Empty;
        public ParameterType Type = ParameterType.Flag;
    }
}

