using System;
using System.Collections.Generic;

namespace NCommander
{
    public static class Splitter
    {
        public static string[] SplitArgs(string input)
        {
            var args = new List<string>();
            var currentArg = new List<char>();
            bool isEscaped = false;
            bool isQuoted = false;
            bool isDoubleQuoted = false;
            int index = -1;
            int lastQuoteIndex = -1;
            bool makeArg = false;

            foreach (char ch_ in input)
            {
                char ch = ch_;
                index++;

                if (char.IsWhiteSpace(ch) && !isQuoted && !isDoubleQuoted)
                {
                    if (currentArg.Count > 0 || makeArg)
                    {
                        args.Add(new string(currentArg.ToArray()));
                        currentArg.Clear();
                    }

                    makeArg = false;
                    continue;
                }

                if (isEscaped)
                {
                    switch (ch)
                    {
                    case 'r':
                        ch = '\r';
                        break;
                    case 'n':
                        ch = '\n';
                        break;
                    case 't':
                        ch = '\t';
                        break;
                    }

                    currentArg.Add(ch);

                    isEscaped = false;
                }
                else if (ch == '\\')
                {
                    isEscaped = true;
                }
                else if (ch == '\'')
                {
                    if (isQuoted)
                    {
                        isQuoted = false;
                    }
                    else if (isDoubleQuoted)
                    {
                        currentArg.Add(ch);
                    }
                    else
                    {
                        isQuoted = true;
                        lastQuoteIndex = index;
                        makeArg = true;
                    }
                }
                else if (ch == '"')
                {
                    if (isDoubleQuoted)
                    {
                        isDoubleQuoted = false;
                    }
                    else if (isQuoted)
                    {
                        currentArg.Add(ch);
                    }
                    else
                    {
                        isDoubleQuoted = true;
                        lastQuoteIndex = index;
                        makeArg = true;
                    }
                }
                else
                {
                    currentArg.Add(ch);
                }
            }

            if (isQuoted || isDoubleQuoted)
            {
                throw new UnmatchedQuoteException(lastQuoteIndex, input[lastQuoteIndex]);
            }

            if (currentArg.Count > 0 || makeArg)
            {
                args.Add(new string(currentArg.ToArray()));
            }

            return args.ToArray();
        }

        public class UnmatchedQuoteException : ApplicationException
        {

            public UnmatchedQuoteException(int index, char delimiter)
                : base(string.Format("Unmatched quote delimiter \"{0}\" at index {1}", delimiter.ToString(), index))
            {
                Index = index;
                Delimiter = delimiter;
            }

            public readonly char Delimiter;
            public readonly int Index;
        }
    }
}

