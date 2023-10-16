namespace Library
{
    public sealed class Library
    {
        public static readonly uint daysOfBorrowWithoutPenalty = 14;
        public static readonly double penaltyAppliedPerDay = 1.0/100;

        private enum Message
        {
            EmptyBookName,
            InexistentBookName,
            InexistentReader,
            InvalidISBN,
            InexistentISBN,
            InvalidRentalPrice,
            AlreadyExistsISBN,
            ZeroCopiesArgument,
            AlreadyRegisteredReader,
            MultipleBooksSameName,
        }

        private static Library? instance = null;
        private Dictionary<string, List<string>> bookNameToISBN;
        private Dictionary<string, Book> bookDetails;
        private Dictionary<string, Reader> readers;

        private Dictionary<Message, string> message = 
            new Dictionary<Message, string>()
            {
                { Message.EmptyBookName, "Book name is empty" },
                { Message.InvalidISBN, "Invalid ISBN" },
                { Message.InvalidRentalPrice , "Rental price is invalid" },
                { Message.AlreadyExistsISBN, "Provided ISBN already exists in the database" },
                { Message.ZeroCopiesArgument, "Number of copies of a book cannot be zero" },
                { Message.AlreadyRegisteredReader, "Reader already registered in the database" },
                { Message.InexistentBookName, "Provided name of the book does not exist in the database" },
                { Message.MultipleBooksSameName, "WARNING: Multiple books with the same name, but different ISBNs" },
                { Message.InexistentISBN, "Provided ISBN does not exist in the database" },
                { Message.InexistentReader, "Reader does not exist in the database" },
            };

        private Library()
        { 
            ResetAttributes();
        }

        public static Library GetInstance
        { 
            get {
                if (instance == null)
                {
                    instance = new Library();
                }
                
                return instance;
            }
        }

        private bool ValidateBookParams(string name, string ISBN, double rentalPrice)
        {
            if (Util.displayMessage(name == null || name == string.Empty, message[Message.EmptyBookName])
                || Util.displayMessage(Util.validateISBN(ISBN) == false, message[Message.InvalidISBN])
                || Util.displayMessage(rentalPrice < 0.0, message[Message.InvalidRentalPrice])) 
                { return false; }

            return true;
        }

        private bool UpdateBookDetails(string ISBN, Book newBook, uint copies)
        {
            bool condition = bookDetails.ContainsKey(ISBN) == true;
            if (Util.displayMessage(condition, message[Message.AlreadyExistsISBN])) { return false; }
            bookDetails[ISBN] = newBook;
            return true;
        }

        private void UpdateBookNameToISBN(string name, string ISBN)
        {
            if (bookNameToISBN.ContainsKey(name) == false)
            {
                bookNameToISBN.Add(name, new List<string>());
            }
            List<string> ISBNs = bookNameToISBN[name];
            if (ISBNs.Contains(ISBN) == false) {
                ISBNs.Add(ISBN);
            }
        }

        public bool AddBook(string name, string ISBN, double rentPrice, uint copies = 1)
        { 
            if (ValidateBookParams(name, ISBN, rentPrice) == false) { return false; }

            Book newBook = new Book(name, ISBN, rentPrice, copies);

            bool retVal = UpdateBookDetails(ISBN, newBook, copies);
            if (retVal == true)
            {
                UpdateBookNameToISBN(name, ISBN);
            }

            return retVal;
        }

        public List<string> GetAllBooks()
        {
            List<string> listOfBooks = new List<string>();
            foreach (Book book in bookDetails.Values)
            {
                listOfBooks.Add(book.ToString());
            }
            return listOfBooks;
        }

        public void AddReader(string name)
        {
            bool condition = readers.ContainsKey(name);
            if (Util.displayMessage(condition, message[Message.AlreadyRegisteredReader])) { return; }
            readers.Add(name, new Reader(name));
        }

        public uint GetRemainingUnitsOfBookByISBN(string ISBN)
        {
            bool condition = bookDetails.ContainsKey(ISBN) == false;
            if (Util.displayMessage(condition, message[Message.InexistentISBN])) { return uint.MaxValue; }

            return bookDetails[ISBN].NumberOfBooks - bookDetails[ISBN].GetNumberOfBorrowers;
        }

        public Dictionary<string, uint>? GetRemainingUnitsOfBook(string name)
        {
            Dictionary<string, uint> result = new Dictionary<string, uint>();
            bool condition = bookNameToISBN.ContainsKey(name) == false;
            if (Util.displayMessage(condition, message[Message.InexistentBookName])) { return result; }

            List<string> ISBNs = bookNameToISBN[name];
            Util.displayMessage(ISBNs.Count > 1, message[Message.MultipleBooksSameName]);
            foreach (string ISBN in ISBNs)
            {
                result[ISBN] = bookDetails[ISBN].NumberOfBooks - bookDetails[ISBN].GetNumberOfBorrowers;
            }

            return result;
        }

        public bool ReturnOrBorrowBookByISBN(string ISBN, string readerID, DateOnly date, BookAction bookAction)
        {
            bool condition = readers.ContainsKey(readerID) == false;
            if (Util.displayMessage(condition, message[Message.InexistentReader])) { return false; }
            Reader reader = readers[readerID];

            condition = bookDetails.ContainsKey(ISBN) == false;
            if (Util.displayMessage(condition, message[Message.InexistentISBN])) { return false; }
            bookAction.BookToModify = bookDetails[ISBN];
            bookAction.Reader = reader;
            bookAction.Date = date;
            bookAction.DoAction();
            return bookAction.result;
        }

        public bool ReturnOrBorrowBook(string name, string readerID, DateOnly date, BookAction bookAction)
        {
            if (Util.displayMessage(bookNameToISBN.ContainsKey(name) == false, message[Message.InexistentBookName])) { return false; }
            List<string> ISBNs = bookNameToISBN[name];

            bool condition = readers.ContainsKey(readerID) == false;
            if (Util.displayMessage(condition, message[Message.InexistentReader])) { return false; }
            
            condition = ISBNs.Count > 1;
            if (Util.displayMessage(condition, message[Message.MultipleBooksSameName]))
            {
                foreach (string ISBN in ISBNs)
                {
                    Console.WriteLine($"\t{ISBN}");
                }
                Console.WriteLine("Please specify the ISBN");
                return false;
            }
            return ReturnOrBorrowBookByISBN(ISBNs[0], readerID, date, bookAction);
        }
        
        public void ResetAttributes()
        {
            bookNameToISBN = new Dictionary<string, List<string>>();
            bookDetails = new Dictionary<string, Book>();
            readers = new Dictionary<string, Reader>();
        }
    }
}
