namespace Library
{
    internal class GetAllBooksCommand : BookCommand
    {
        List<string>? allBooks;

        public void PrintListOfBooks()
        {
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
