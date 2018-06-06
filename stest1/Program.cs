using System;
using System.Collections.Generic;
using System.Linq;
using Sodium;

namespace stest1
{
    class Program
    {
        static readonly Cell<List<string>> cGreetings = Cell.Constant(new List<string> { "hello", "hey", "hi", "sup", "howdy", "word", "greetings earthling" });
        static readonly Cell<List<string>> cGoodbye = Cell.Constant(new List<string> { "goodbye", "bye", "later", "exit", "ciao", "out", "adios" }); 

        private static void Main(string[] args)
        {
            CellSink<string> csInput = new CellSink<string>(string.Empty);
            Cell<string> cInput = csInput.Map(x => x);
            Cell<List<string>> tokens = cInput.Map(Tokenize);

            Cell<string> cOutput = cInput.Map(GetResponse);
            Cell<bool> exit = cInput.Map(v => cGoodbye.Sample().Contains(v));
            Cell<bool> randomWord = cInput.Map(i => i.Contains("random"));
             
            while (!exit.Sample())
            {
                W("You: ");
                csInput.Send(Console.ReadLine());
                W("SodiumBot: ");
                Wl(randomWord.Sample() ? GetRandomIndexOf(tokens.Sample()) : cOutput.Sample());
                Wl(tokens.Sample().Count > 3 ? "word count: " + tokens.Sample().Count : string.Empty);
            }
            Console.ReadKey();
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
            return cGreetings.Sample().Contains(t);
        }

        private static bool IsGoodbye(string t)
        {
            return cGoodbye.Sample().Contains(t);
        }

        private static string GetGreeting()
        {
            return GetRandomIndexOf(cGreetings.Sample());
        }

        private static string GetGoodbye()
        {
            return GetRandomIndexOf(cGoodbye.Sample());
        }

        private static string GetRandomIndexOf(List<string> coll)
        {
            var topRange = coll.Count;
            var r = new Random();
            var i = r.Next(0, topRange);
            return coll[i];
        }

        private static List<string> Tokenize(string input)
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