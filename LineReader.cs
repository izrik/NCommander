using System;
using System.Collections.Generic;
using System.Threading;

namespace NCommander
{
    public static class LineReader
    {
        public static string ReadLine(string prompt)
        {
            Console.Write(prompt);
            try
            {
                return Console.ReadLine();
            }
            catch (ThreadAbortException) // Ctrl + C
            {
                Thread.ResetAbort();
                return "";
            }
        }
    }
}
