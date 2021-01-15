using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace Std
{
    public sealed class StdOut
    {
        private StdOut() { }

        private static readonly Stream stream;

        public static void println()
        {
            Console.WriteLine();
        }


        public static void println(object x)
        {
            Console.WriteLine(x);
        }

        public static void println(bool x)
        {
            Console.WriteLine(x);
        }

        public static void println(char x)
        {
            Console.WriteLine(x);
        }

        public static void println(double x)
        {
            Console.WriteLine(x);
        }

        public static void println(float x)
        {
            Console.WriteLine(x);
        }

        public static void println(int x)
        {
            Console.WriteLine(x);
        }

        public static void println(long x)
        {
            Console.WriteLine(x);
        }

        public static void println(short x)
        {
            Console.WriteLine(x);
        }

        public static void println(byte x)
        {
            Console.WriteLine(x);
        }

        public static void print()
        {
            stream.Flush();
        }

        public static void print(object x)
        {
            Console.Write(x);
            stream.Flush();
        }

        public static void print(bool x)
        {
            Console.Write(x);
            stream.Flush();
        }

        public static void print(char x)
        {
            Console.Write(x);
            stream.Flush();
        }

        public static void print(double x)
        {
            Console.Write(x);
            stream.Flush();
        }

        public static void print(float x)
        {
            Console.Write(x);
            stream.Flush();
        }

        public static void print(int x)
        {
            Console.Write(x);
            stream.Flush();
        }


        public static void print(long x)
        {
            Console.Write(x);
            stream.Flush();
        }


        public static void print(short x)
        {
            Console.Write(x);
            stream.Flush();
        }


        public static void print(byte x)
        {
            Console.Write(x);
            stream.Flush();
        }


        public static void printf(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            stream.Flush();
        }

        public static void main(string[] args)
        {
            // write to stdConsole
            println("Test");
            println(17);
            println(true);
            printf("%.6f\n", 1.0 / 7.0);
        }

    }
}