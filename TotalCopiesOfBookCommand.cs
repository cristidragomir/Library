using System.Text.RegularExpressions;

namespace Library
{
    internal class TotalCopiesOfBookCommand : BookCommand
    {
        private uint result;
        private Dictionary<string, uint>? nameCollision;
        private static List<string> options =
            new List<string>() { "name", "code" };
        private static readonly string commandPattern =
            @"^\s*(\w+)\s+\-\-(name|code)\s+""([\-\\\{\}\[\]\d()',?!;:\.\w\s]*)""\s*$";

        private enum SpecificMsg
        {
            UnknownOption
        }

        private readonly Dictionary<SpecificMsg, string> specificMsg =
            new Dictionary<SpecificMsg, string>()
            {
                { SpecificMsg.UnknownOption, $"Unknown option. Available options are: { string.Join(", ", options) }"}
            };

        private void PrintNameCollisions(string bookName)
        {
            Console.WriteLine("Books with the same name " + "\"" + bookName + "\"" + " but different ISBNs:");
            foreach (string key in nameCollision.Keys)
            {
                Console.WriteLine("\t" + key + " : " + nameCollision[key]);
            }
        }

        public override void DoAction()
        {
            Match match = CheckCommandCorrectness(commandPattern);
            if (match == null) { return; }

            string option = match.Groups[2].Value;
            string argument = match.Groups[3].Value;

            if (Util.displayMessage(argument.Equals(""), message[Message.EmptyArg])) { return; }

            if (option.Equals("name"))
            {
                nameCollision = libInstance.GetRemainingUnitsOfBook(argument);
                if (nameCollision.Count == 1)
                {
                    string key = nameCollision.Keys.First();
                    Console.WriteLine($"Remaining copies of the book with name: \"{argument}\" is {nameCollision[key]}");
                }
                else if (nameCollision.Count > 1)
                {
                    PrintNameCollisions(argument);
                }
            } else if (option.Equals("code"))
            {
                result = libInstance.GetRemainingUnitsOfBookByISBN(argument);
                if (result < uint.MaxValue)
                {
                    Console.WriteLine($"Remaining copies of the book with ISBN=\"{argument}\" is {result}");
                }
            } else
            {
                Console.WriteLine(specificMsg[SpecificMsg.UnknownOption]);
                return;
            }
        }
    }
}