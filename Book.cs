namespace Library
{
    public class Book
    {
        private string name;
        private string ISBN;
        private double rentPrice;
        private uint numberOfBooks;
        private Dictionary<Reader, DateOnly> borrowers;

        public Book(string name, string ISBN, double rentPrice)
        { 
            this.name = name;
            this.ISBN = ISBN;
            this.rentPrice = rentPrice;
            numberOfBooks = 1;
            borrowers = new Dictionary<Reader, DateOnly>();
        }

        public Book(string name, string ISBN, double rentPrice, uint numberOfBooks) : this(name, ISBN, rentPrice)
        {
            this.numberOfBooks = numberOfBooks;
        }

        public uint NumberOfBooks
        {
            get { return numberOfBooks; }
            set { numberOfBooks = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string GetISBN
        {
            get { return ISBN; }
        }

        public uint GetNumberOfBorrowers
        {
            get { return (uint)borrowers.Count; }
        }

        public void BorrowBook(Reader reader, DateOnly date)
        {
            uint availableCopies = numberOfBooks - GetNumberOfBorrowers;

            if (borrowers.ContainsKey(reader))
            {
                Console.WriteLine("The reader has already borrowed a copy of this book");
                return;
            }
            
            if (availableCopies == 0)
            {
                Console.WriteLine("There are no more copies available for this book + [" + ToString() + "]");
                return;
            }

            borrowers.Add(reader, date);
        }

        public void ReturnBook(Reader reader, DateOnly date)
        {
            if (borrowers.ContainsKey(reader) == false)
            {
                Console.WriteLine("WARNING: This reader is not registered as a borrower of this book");
                return;
            }

            DateOnly dateOfBorrow = borrowers[reader];
            DateTime dateOfBorrowAdvanced = dateOfBorrow.ToDateTime(new TimeOnly(0));
            DateTime dateOfReturnAdvanced = date.ToDateTime(new TimeOnly(0));

            int periodOfBorrow = (dateOfReturnAdvanced - dateOfBorrowAdvanced).Days;

            if (periodOfBorrow <= Library.daysOfBorrowWithoutPenalty)
            {
                Console.WriteLine($"Reader {reader.Name} has to pay {rentPrice} without any penalties");
            } else if (periodOfBorrow > Library.daysOfBorrowWithoutPenalty)
            {
                double penaltyToPay = (periodOfBorrow - Library.daysOfBorrowWithoutPenalty) * (Library.penaltyAppliedPerDay * rentPrice);
                double totalCost = rentPrice + penaltyToPay;
                Console.WriteLine($"Reader {reader.Name} has to pay {rentPrice} and additional penalty of {penaltyToPay}. TOTAL: {totalCost}");
            }

            borrowers.Remove(reader);
        }

        public override string ToString()
        {
            return "ISBN = " + ISBN + ", " + "Name = " + name + ", " + "Rent price = " + rentPrice;
        }
    }
}
