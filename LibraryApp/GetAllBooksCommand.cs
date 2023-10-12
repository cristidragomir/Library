namespace Library
{
    public class GetAllBooksCommand : BookCommand
    {
        List<string>? allBooks;

        public void PrintListOfBooks()
        {
            if (allBooks.Count == 0)
            {
                Console.WriteLine("No books in the library"); 
                return; 
            }
            Console.WriteLine();
            for (int i = 0; i < allBooks.Count; i++)
            {
                string bookDetails = allBooks[i];
                Console.WriteLine(bookDetails);
            }
            Console.WriteLine();
        }
        public override void DoAction()
        {
            allBooks = libInstance.GetAllBooks();
            PrintListOfBooks();
        }
    }
}
