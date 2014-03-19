using System;
using System.Collections.Generic;
using System.Linq;

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
                var rawTopic = (string)(args["topic"]);
                var topics = new [] { rawTopic, rawTopic.ToLower() };
                var types = _commander.GetAllParameterTypes();

                foreach (var topic in topics)
                {
                    if (_commander.Commands.ContainsKey(topic))
                    {
                        GetHelpForCommand(_commander.Commands[topic]);
                        return;
                    }

                    if (_commander.HelpTopics.ContainsKey(topic))
                    {
                        _commander.HelpTopics[topic]();
                        return;
                    }

                    if (types.FirstOrDefault(x => x.Name == topic) != null)
                    {
                        GetHelpForType(types.First(x => x.Name == topic));
                        return;
                    }
		}

                Console.WriteLine("Unknown topic: \"{0}\"", rawTopic);
                _commander.ShowUsage();
            }
            else
            {
                _commander.ShowGeneralHelp();
            }
        }

        protected void GetHelpForCommand(Command command)
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

        void GetHelpForType(ParameterType parameterType)
        {
            Console.Write(parameterType.Name);

            if (!string.IsNullOrWhiteSpace(parameterType.Description))
            {
                Console.Write(" - {0}", parameterType.Description);
            }

            Console.WriteLine();
            Console.WriteLine();

            if (!string.IsNullOrWhiteSpace(parameterType.HelpText))
            {
                Console.WriteLine(parameterType.HelpText);
                Console.WriteLine();
            }
        }
    }
}

