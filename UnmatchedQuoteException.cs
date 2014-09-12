using System;

namespace NCommander
{
    public class UnmatchedQuoteException : Exception
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

