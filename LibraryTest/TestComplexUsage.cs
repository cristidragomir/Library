using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryTest
{
    [TestClass]
    public class TestComplexUsage
    {
        [TestMethod]
        public void TestNormalFlow()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "getallbooks",
                "addbook \"Carte'a exemplu-1. Subtitlu\" \"978 10 596 52068 7\" 12.34 4", // Invalid ISBN
                "addbook \"Carte'a exemplu-1. Subtitlu\" \"978 0 596 52068 7\" 12.34 4",
                "addbook \"Carte'a exemplu-1. Subtitlu\" \"ISBN 978-0-496-52068-7\" 75 2",
                "addbook \"Carte'a exemplu-4. Subtitlu\" \"978 0 596 32168 5\" 34",
                "addbook \"Carte'a exemplu-2. Subtitlu\" \"978 0 596 32168 5\" 45.22 3", // duplicate ISBN
                "addbook \"Carte'a exemplu-2. Subtitlu\" \"978 0 216 12144 5\" 45.22 3",
                "getallbooks", // Check if books have been introduced correctly
                "availablecopies --name \"Carte'a exemplu-4. Subtitlu\"",
                "borrowbookto --name \"Carte'a exemplu-4. Subtitlu\" \"Adrian\"  1/10/2023",
                "availablecopies --name \"Carte'a exemplu-1. Subtitlu\"", // multiple books with the same name
                "availablecopies --code \"ISBN 978-0-496-52068-7\"",
                "borrowbookto --code \"ISBN 978-0-496-52068-7\" \"Adrian\"  2/10/2023",
                "borrowbookto --code \"ISBN 978-0-496-52068-7\" \"Georgeta\"  2/10/2023",
                "borrowbookto --code \"ISBN 978-0-496-52068-7\" \"Bogdan\"  2/10/2023", // no more copies for the book
                "getallbooks",
                "borrowbookto --name \"Carte'a exemplu-4. Subtitlu\" \"Adrian\"  4/10/2023", // already borrowed to this user
                "borrowbookto --name \"Carte'a exemplu-4. Subtitlu\" \"Georgeta\"  4/10/2023", // no more copies
                "availablecopies --name \"Carte'a exemplu-1. Subtitlu\"",
                "returnbookfrom --name \"Carte'a exemplu-4. Subtitlu\" \"Mihaela\"  30/10/2023", // the reader hasn't borrowed this book
                "returnbookfrom --name \"Carte'a exemplu-4. Subtitlu\" \"Adrian\"  30/10/2023",
                "availablecopies --name \"Carte'a exemplu-4. Subtitlu\"",
                "exit\n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "No books in the library",
                "Invalid ISBN",
                "Book added successfully",
                "Book added successfully",
                "Book added successfully",
                "Provided ISBN already exists in the database",
                "Book added successfully",
                "",
                "ISBN = 978 0 596 52068 7, Name = Carte'a exemplu-1. Subtitlu, Rent price = 12.34",
                "ISBN = ISBN 978-0-496-52068-7, Name = Carte'a exemplu-1. Subtitlu, Rent price = 75",
                "ISBN = 978 0 596 32168 5, Name = Carte'a exemplu-4. Subtitlu, Rent price = 34",
                "ISBN = 978 0 216 12144 5, Name = Carte'a exemplu-2. Subtitlu, Rent price = 45.22",
                "",
                "Remaining copies of the book with name: \"Carte'a exemplu-4. Subtitlu\" is 1",
                "Book successfully borrowed",
                "WARNING: Multiple books with the same name, but different ISBNs",
                "Books with the same name \"Carte'a exemplu-1. Subtitlu\" but different ISBNs:",
                "\t978 0 596 52068 7 : 4",
                "\tISBN 978-0-496-52068-7 : 2",
                "Remaining copies of the book with ISBN: \"ISBN 978-0-496-52068-7\" is 2",
                "Book successfully borrowed",
                "Book successfully borrowed",
                "There are no more copies available for this book + [ISBN = ISBN 978-0-496-52068-7, Name = Carte'a exemplu-1. Subtitlu, Rent price = 75]",
                "",
                "ISBN = 978 0 596 52068 7, Name = Carte'a exemplu-1. Subtitlu, Rent price = 12.34",
                "ISBN = ISBN 978-0-496-52068-7, Name = Carte'a exemplu-1. Subtitlu, Rent price = 75",
                "ISBN = 978 0 596 32168 5, Name = Carte'a exemplu-4. Subtitlu, Rent price = 34",
                "ISBN = 978 0 216 12144 5, Name = Carte'a exemplu-2. Subtitlu, Rent price = 45.22",
                "",
                "The reader has already borrowed a copy of this book",
                "There are no more copies available for this book + [ISBN = 978 0 596 32168 5, Name = Carte'a exemplu-4. Subtitlu, Rent price = 34]",
                "WARNING: Multiple books with the same name, but different ISBNs",
                "Books with the same name \"Carte'a exemplu-1. Subtitlu\" but different ISBNs:",
                "\t978 0 596 52068 7 : 4",
                "\tISBN 978-0-496-52068-7 : 0",
                "WARNING: This reader is not registered as a borrower of this book",
                "Reader Adrian has to pay 34 and additional penalty of 5.1 (15 days). TOTAL: 39.1",
                "Book successfully returned",
                "Remaining copies of the book with name: \"Carte'a exemplu-4. Subtitlu\" is 1",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }
    }
}
