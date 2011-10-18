using System;

namespace Ekra3.BDDviaNUnit.OSpec
{
    static class ConsoleHelper
    {
        public static void WriteLineUnderlining(char underlineChar, string format, params object[] args)
        {
            var text = string.Format(format, args);
            Console.WriteLine(text);
            Console.WriteLine("{0}", "".PadLeft(text.Length, underlineChar));
        }
        public static void WriteLineUnderlining(char underlineChar, int underlineLength, string format, params object[] args)
        {
            var text = string.Format(format, args);
            Console.WriteLine(text);
            Console.WriteLine("{0}", "".PadLeft(underlineLength, underlineChar));
        }
    }
}