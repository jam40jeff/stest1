using System;
using System.Collections.Generic;
using System.Linq;
using Sodium;

namespace stest1
{
    class Program
    {
        private static readonly IReadOnlyList<string> cGreetings =
            new[] {"hello", "hey", "hi", "sup", "howdy", "word", "greetings earthling"};

        private static readonly IReadOnlyList<string> cGoodbye = new[]
            {"goodbye", "bye", "later", "exit", "ciao", "out", "adios"};

        private static void Main(string[] args)
        {
            StreamSink<string> ssInput = new StreamSink<string>();
            Stream<IReadOnlyList<string>> sOutput = ssInput.Map(GetOutput);
            Cell<bool> cExit = ssInput.Map(v => cGoodbye.Any(g => v == g)).Hold(false);

            using (sOutput.Listen(
                outputLines =>
                {
                    foreach (string outputLine in outputLines)
                    {
                        Wl(outputLine);
                    }
                }))
            {
                while (!cExit.Sample())
                {
                    W("You: ");
                    ssInput.Send(Console.ReadLine());
                }
            }

            Wl(string.Empty);
            Wl("Press enter to exit...");
            Console.ReadLine();
        }

        static IReadOnlyList<string> GetOutput(string input)
        {
            IReadOnlyList<string> tokens = Tokenize(input);

            return new[] {input.Contains("random") ? GetRandomIndexOf(tokens) : GetResponse(input)}.Concat(
                tokens.Count > 3
                    ? new[] {"word count: " + tokens.Count}
                    : new string[0]).ToArray();
        }

        static string GetResponse(string input)
        {
            if (IsGreeting(input))
                return GetGreeting();

            if (IsGoodbye(input))
                return GetGoodbye();

            return "default response";
        }

        private static bool IsGreeting(string t)
        {
            return cGreetings.Any(v => v == t);
        }

        private static bool IsGoodbye(string t)
        {
            return cGoodbye.Any(v => v == t);
        }

        private static string GetGreeting()
        {
            return GetRandomIndexOf(cGreetings);
        }

        private static string GetGoodbye()
        {
            return GetRandomIndexOf(cGoodbye);
        }

        private static string GetRandomIndexOf(IReadOnlyList<string> coll)
        {
            var topRange = coll.Count;
            var r = new Random();
            var i = r.Next(0, topRange);
            return coll[i];
        }

        private static IReadOnlyList<string> Tokenize(string input)
        {
            return input.Split(' ').ToList();
        }

        private static void Wl(string x)
        {
            Console.WriteLine(x);
        }

        private static void W(string x)
        {
            Console.Write(x);
        }
    }
}