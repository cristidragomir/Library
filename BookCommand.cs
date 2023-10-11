using System.Globalization;
using System.Text.RegularExpressions;

namespace Library
{
    internal abstract class BookCommand : IAction
    {
        public delegate void BookManipulationDelegate(string name, string readerID, DateOnly date, BookAction bookAction);

        private static Dictionary<string, BookManipulationDelegate> options =
            new Dictionary<string, BookManipulationDelegate>
            {
                { "name", libInstance.ReturnOrBorrowBook },
                { "code", libInstance.ReturnOrBorrowBookByISBN },
            };
        protected enum Message
        {
            ImproperCommand,
            EmptyArg,
            UnknownOption,
            InvalidDate,
        }

        protected readonly Dictionary<Message, string> message =
            new Dictionary<Message, string>()
            {
                { Message.EmptyArg, "Given argument is an empty string" },
                { Message.ImproperCommand, "Command is not given in proper format" },
                { Message.UnknownOption, $"Unknown option. Available options are: { string.Join(", ", options.Keys) }" },
                { Message.InvalidDate, $"Provided date is invalid" },
            };

        protected string? fullCommand;
        protected static Library libInstance = Library.GetInstance;

        public string FullCommand { set { fullCommand = value; } }

        public abstract void DoAction();

        protected Match? CheckCommandCorrectness(string commandPattern)
        {
            bool condition = Util.IsStringMatchingPattern(commandPattern, fullCommand) == false;
            if (Util.displayMessage(condition, message[Message.ImproperCommand])) { return null; }

            MatchCollection matchCollection = Regex.Matches(fullCommand, commandPattern);
            
            return matchCollection[0];
        }

        protected bool BorrowReturnBook(string commandPattern, BookAction bookAction)
        {
            Match match = CheckCommandCorrectness(commandPattern);
            if (match == null) { return false; }

            string bookOption = match.Groups[2].Value;
            string bookIdentifier = match.Groups[3].Value;
            string readerID = match.Groups[4].Value;
            string date = match.Groups[5].Value;

            CultureInfo culture = new CultureInfo("ro-RO");
            DateOnly dateParsed;

            bool evaluation = DateOnly.TryParse(date, culture, DateTimeStyles.None, out dateParsed);

            if (Util.displayMessage(bookIdentifier.Equals(""), message[Message.EmptyArg])
                || Util.displayMessage(readerID.Equals(""), message[Message.EmptyArg])
                || Util.displayMessage(evaluation == false, message[Message.InvalidDate])
                || Util.displayMessage(options.ContainsKey(bookOption) == false, message[Message.UnknownOption])) { return false; }

            options[bookOption](bookIdentifier, readerID, dateParsed, bookAction);

            return true;
        }
    }
}
