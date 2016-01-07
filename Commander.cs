using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace NCommander
{
    public class Commander
    {
        public Commander(string programName, string programVersion, bool addHelpCommand=true, TextWriter output=null)
        {
            if (string.IsNullOrWhiteSpace(programName)) throw new ArgumentNullException("programName");
            if (string.IsNullOrWhiteSpace(programVersion)) throw new ArgumentNullException("programVersion");

            if (output == null) output = Console.Out;

            this.ProgramName = programName;
            this.ProgramVersion = programVersion;

            if (addHelpCommand)
            {
                Commands.Add("help", new HelpCommand(this));
            }

            Output = output;
        }

        public readonly string ProgramName;
        public readonly string ProgramVersion;
        public readonly Dictionary<string, Command> Commands = new Dictionary<string, Command>();
        public readonly Dictionary<string, Action> HelpTopics = new Dictionary<string, Action>();
        public readonly TextWriter Output;

        public Action AdditionalUsage;

        public void ProcessArgs(params string[] args)
        {
            ProcessArgs((IEnumerable<string>)args);
        }
        public void ProcessArgs(IEnumerable<string> args)
        {
            var args2 = args.ToList();

            var commandName = args2[0];
            args2.RemoveAt(0);

            if (Commands.ContainsKey(commandName))
            {
                Commands[commandName].Execute(args2);
            }
            else
            {
                Output.WriteLine("Unknown command, \"{0}\"", commandName);
                ShowUsage();
            }
        }


        public void ShowVersion()
        {
            Output.WriteLine("{0} version {1}", this.ProgramName, this.ProgramVersion);
        }

        public void ShowUsage()
        {
            Output.WriteLine("Usage:");
            Output.WriteLine("    {0} [options]", this.ProgramName);
            Output.WriteLine("    {0} help [command_or_topic]", this.ProgramName);
            Output.WriteLine("    {0} command [args...]", this.ProgramName);
            Output.WriteLine();

            if (AdditionalUsage != null)
            {
                AdditionalUsage();
            }
        }

        public void ShowGeneralHelp()
        {
            ShowUsage();

            if (Commands.Count > 0)
            {
                Output.WriteLine("Commands:");
                Output.WriteLine();
                foreach (var kvp in Commands)
                {
                    Output.WriteLine("    {0,-10} {1}", kvp.Key, kvp.Value.Description);
                }
                Output.WriteLine();
            }

            if (HelpTopics.Count > 0)
            {
                Output.WriteLine("Help topics:");
                Output.WriteLine();
                foreach (var topic in HelpTopics.Keys)
                {
                    Output.WriteLine("    {0}", topic);
                }
                Output.WriteLine();
            }

            var types = GetAllParameterTypes();
            if (types.Length > 0)
            {
                Output.WriteLine("Types:");
                Output.WriteLine();
                foreach (var type in types)
                {
                    Output.WriteLine("    {0,-10} {1}", type.Name, type.Description);
                }
                Output.WriteLine();
            }
        }

        public ParameterType[] GetAllParameterTypes()
        {
            return Commands.Values.SelectMany(x => x.Params).Select(x => x.ParameterType).Distinct().ToArray();
        }
    }
}

