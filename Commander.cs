using System;
using System.Collections.Generic;
using System.Linq;

namespace NCommander
{
    public class Commander
    {
        public Commander(string programName, string programVersion, bool addHelpCommand=true)
        {
            if (string.IsNullOrWhiteSpace(programName)) throw new ArgumentNullException("programName");
            if (string.IsNullOrWhiteSpace(programVersion)) throw new ArgumentNullException("programVersion");

            this.ProgramName = programName;
            this.ProgramVersion = programVersion;

            if (addHelpCommand)
            {
                Commands.Add("help", new HelpCommand(this));
            }
        }

        public readonly string ProgramName;
        public readonly string ProgramVersion;
        public readonly Dictionary<string, Command> Commands = new Dictionary<string, Command>();
        public readonly Dictionary<string, Action> HelpTopics = new Dictionary<string, Action>();

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
                Console.WriteLine("Unknown command, \"{0}\"", commandName);
                ShowUsage();
            }
        }


        public void ShowVersion()
        {
            Console.WriteLine("{0} version {1}", this.ProgramName, this.ProgramVersion);
        }

        public void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("    {0} [options]", this.ProgramName);
            Console.WriteLine("    {0} help [command_or_topic]", this.ProgramName);
            Console.WriteLine("    {0} command [args...]", this.ProgramName);
            Console.WriteLine();

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
                Console.WriteLine("Commands:");
                Console.WriteLine();
                foreach (var kvp in Commands)
                {
                    Console.WriteLine("    {0,-10} {1}", kvp.Key, kvp.Value.Description);
                }
                Console.WriteLine();
            }

            if (HelpTopics.Count > 0)
            {
                Console.WriteLine("Help topics:");
                Console.WriteLine();
                foreach (var topic in HelpTopics.Keys)
                {
                    Console.WriteLine("    {0}", topic);
                }
                Console.WriteLine();
            }

            var types = GetAllParameterTypes();
            if (types.Length > 0)
            {
                Console.WriteLine("Types:");
                Console.WriteLine();
                foreach (var type in types)
                {
                    Console.WriteLine("    {0,-10} {1}", type.Name, type.Description);
                }
                Console.WriteLine();
            }
        }

        public ParameterType[] GetAllParameterTypes()
        {
            return Commands.Values.SelectMany(x => x.Params).Select(x => x.ParameterType).Distinct().ToArray();
        }
    }
}

