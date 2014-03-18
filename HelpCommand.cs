using System;
using System.Collections.Generic;

namespace NCommander
{
    public class HelpCommand : Command
    {
        public HelpCommand(string programName, string programVersion, IDictionary<string, Command> commands=null, IDictionary<string, Action> helpTopics=null)
        {
            if (string.IsNullOrWhiteSpace(programName)) throw new ArgumentNullException("programName");
            if (string.IsNullOrWhiteSpace(programVersion)) throw new ArgumentNullException("programVersion");

            this.ProgramName = programName;
            this.ProgramVersion = programVersion;
            this.Commands = commands;
            this.HelpTopics = helpTopics;

            Name = "help";
            Description = "Display general help, or help on a specific topic.";
            Params = new [] {
                new Parameter { Name = "topic", ParameterType = ParameterType.String, IsOptional = true },
            };
        }

        public readonly string ProgramName;
        public readonly string ProgramVersion;
        public readonly IDictionary<string, Command> Commands;
        public readonly IDictionary<string, Action> HelpTopics;

        protected override void InternalExecute(Dictionary<string, object> args)
        {
            if (args.ContainsKey("topic"))
            {
                var topic = ((string)(args["topic"])).ToLower();

                if (Commands.ContainsKey(topic))
                {
                    GetHelp(Commands[topic]);
                }
                else if (HelpTopics.ContainsKey(topic))
                {
                    HelpTopics[topic]();
                }
                else
                {
                    Console.WriteLine("Unknown topic: \"{0}\"", topic);
                    ShowUsage();
                }
            }
            else
            {
                ShowGeneralHelp();
            }
        }

        protected void GetHelp(Command command)
        {
            if (command.HelpAction != null)
            {
                command.HelpAction();
            }
            else
            {
                Console.WriteLine("Usage:");
                Console.Write("    {0} {1}", this.ProgramName, command.Name);
                foreach (var param in command.Params)
                {
                    if (param.IsOptional)
                    {
                        Console.Write(" [{0}]", param.Name);
                    }
                    else
                    {
                        Console.Write(" <{0}>", param.Name);
                    }
                }
                Console.WriteLine();
                Console.WriteLine();

                if (!string.IsNullOrEmpty(command.Description))
                {
                    Console.WriteLine(command.Description);
                    Console.WriteLine();
                }

                if (command.Params.Length > 0)
                {
                    Console.WriteLine("Parameters:");
                    Console.WriteLine();

                    foreach (var param in command.Params)
                    {
                        Console.WriteLine(
                            "    {0,-10} {1}{2} - {3}", 
                            param.Name,
                            (param.IsOptional ? "Optional " : ""),
                            param.ParameterType,
                            param.Description);
                    }
                    Console.WriteLine();
                }

                if (!string.IsNullOrEmpty(command.HelpText))
                {
                    Console.WriteLine(command.HelpText);
                    Console.WriteLine();
                }
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

            // TODO: Print NDesk.Options options here

            // Console.WriteLine("Options:");
            // Console.WriteLine();
            // _options.WriteOptionDescriptions(Console.Out);
            // Console.WriteLine();
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
                Console.WriteLine("Additional help topics:");
                Console.WriteLine();
                foreach (var topic in HelpTopics.Keys)
                {
                    Console.WriteLine("    {0}", topic);
                }
                Console.WriteLine();
            }
        }
    }
}

