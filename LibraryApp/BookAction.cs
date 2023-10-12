namespace Library
{
    public abstract class BookAction : IAction
    {
        protected Book? bookToModify;
        protected Reader? reader;
        protected DateOnly date = DateOnly.MinValue;

        public BookAction() { }
        
        public Book BookToModify { set { bookToModify = value; } }
        public Reader Reader { set { reader = value; } }
        public DateOnly Date { set { date = value; } }

        public bool CheckNullValues()
        {
            if (bookToModify == null)
            {
                Console.WriteLine("No book to operate with");
                return false;
            }
            if (reader == null)
            {
                Console.WriteLine("No reader to operate with");
                return false;
            }
            if (date == DateOnly.MinValue)
            {
                Console.WriteLine("No date provided");
                return false;
            }
            return true;
        }

        public abstract void DoAction();
    }
}
