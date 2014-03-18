using System;
using System.Collections.Generic;

namespace NCommander
{
    public class HelpCommand : Command
    {
        public HelpCommand(Commander commander)
        {
            if (commander == null) throw new ArgumentNullException("commander");

            _commander = commander;

            Name = "help";
            Description = "Display general help, or help on a specific topic.";
            Params = new [] {
                new Parameter { Name = "topic", ParameterType = ParameterType.String, IsOptional = true },
            };
        }

        readonly Commander _commander;

        protected override void InternalExecute(Dictionary<string, object> args)
        {
            if (args.ContainsKey("topic"))
            {
                var topic = ((string)(args["topic"])).ToLower();

                if (_commander.Commands.ContainsKey(topic))
                {
                    GetHelp(_commander.Commands[topic]);
                }
                else if (_commander.HelpTopics.ContainsKey(topic))
                {
                    _commander.HelpTopics[topic]();
                }
                else
                {
                    Console.WriteLine("Unknown topic: \"{0}\"", topic);
                    _commander.ShowUsage();
                }
            }
            else
            {
                _commander.ShowGeneralHelp();
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
                Console.Write("    {0} {1}", _commander.ProgramName, command.Name);
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
    }
}

