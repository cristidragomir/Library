using System.Text.RegularExpressions;

namespace Library
{
    public class AddBookCommand : BookCommand
    {
        private readonly string commandPattern =
            @"^\s*(\w+)\s+""([\-\\\{\}\[\]\d()',?!;:\.\w\s]*)""\s+""([\:\-\w\s]*)""\s+([\d.]+)\s*(?:\s(\d+))?\s*$";

        private enum SpecificMsg
        {
            InvalidPrice,
            InvalidNumberOfCopies
        }

        private readonly Dictionary<SpecificMsg, string> specificMsg = 
            new Dictionary<SpecificMsg, string>()
            {
                { SpecificMsg.InvalidPrice, "Price is not provided in the correct format" },
                { SpecificMsg.InvalidNumberOfCopies, "Number of units provided not in the correct format" }
            };

        public override void DoAction()
        {
            Match match = CheckCommandCorrectness(commandPattern);
            if (match == null) { return; }

            string bookName = match.Groups[2].Value;
            string ISBN = match.Groups[3].Value;
            double rentalPrice;
            
            bool evaluation = double.TryParse(match.Groups[4].Value, out rentalPrice);   
            if (Util.displayMessage(evaluation == false, specificMsg[SpecificMsg.InvalidPrice])) { return; }

            uint countCopies = 1;
            if (match.Groups[5].Value.Equals("") == false)
            {
                string strCountCopiesValue = match.Groups[5].Value;
                evaluation = uint.TryParse(strCountCopiesValue, out countCopies);
                if (Util.displayMessage(evaluation == false, specificMsg[SpecificMsg.InvalidNumberOfCopies])) { return; }
                if (strCountCopiesValue.Equals("")) { countCopies = 1; }
            }
            libInstance.AddBook(bookName, ISBN, rentalPrice, countCopies);
            Console.WriteLine("Book added successfully");
        }
    }
}
