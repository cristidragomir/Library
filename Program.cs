namespace Library
{
    internal class Program
    {
        public static Library? lib;
        public static Dictionary<string, BookCommand>? commandToExecute;

        public static void InitLibrary()
        {
            lib = Library.GetInstance;
            commandToExecute = new Dictionary<string, BookCommand>();

            lib.AddReader("Adrian");
            lib.AddReader("Bogdan");
            lib.AddReader("Georgeta");
            lib.AddReader("Mihaela");
            lib.AddReader("Alex");
            lib.AddReader("Claudiu");

            commandToExecute["addbook"] = new AddBookCommand();
            commandToExecute["getallbooks"] = new GetAllBooksCommand();
            commandToExecute["availablecopies"] = new TotalCopiesOfBookCommand();
            commandToExecute["borrowbookto"] = new BorrowBookCommand();
            commandToExecute["returnbookfrom"] = new ReturnBookCommand();
        }
        
        static void Main(string[] args)
        {
            InitLibrary();
            if (commandToExecute == null) { return; }

            string? command = Console.ReadLine();
            
            while (command != null && command.Trim().Equals("exit") == false)
            {
                string[] tokens = command.Trim().Split(" ");
                if (commandToExecute.ContainsKey(tokens[0]) == false)
                {
                    Console.WriteLine($"\"{command}\" is not a valid command");
                } else 
                {
                    commandToExecute[tokens[0]].FullCommand = command;
                    commandToExecute[tokens[0]].DoAction();
                }
                command = Console.ReadLine();
            }
        }
    }
}