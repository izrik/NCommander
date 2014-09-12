using System;

namespace NCommander
{
    public class UnmatchedQuoteException : NCommanderException
    {
        public UnmatchedQuoteException(int index, char delimiter)
        {
            Index = index;
            Delimiter = delimiter;
        }

        public readonly char Delimiter;
        public readonly int Index;

        public override string Message
        {
            get
            {
                return string.Format("Unmatched quote delimiter \"{0}\" at index {1}", Delimiter.ToString(), Index);
            }
        }
    }
}

