using Library;
using System.Security.Policy;
using System.Xml.Linq;

namespace LibraryTest
{
    [TestClass]
    public class BasicTests
    {
        static TextReader input;
        static StringWriter output;

        private static void SetConsoleIO(string inputStr)
        {
            input = new StringReader(inputStr);
            output = new StringWriter();

            Console.SetIn(input);
            Console.SetOut(output);
        }

        public static void RunTest(List<string> arrayOfCommands, List<string> expectedOutputLines)
        {
            SetConsoleIO(string.Join("\n", arrayOfCommands));

            Program.Main(null);

            var result = output.ToString().Replace("\r", "");
            Assert.AreEqual(string.Join("\n", expectedOutputLines), result);

            input.Dispose();
            output.Dispose();
        }

        [TestMethod]
        public void TestInexistentCommand()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                " kjfjkrje  ",
                "exit\n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "\" kjfjkrje  \" is not a valid command",
                ""
            };

            RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestNoBooksInTheLibrary()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "getallbooks",
                "exit\n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "No books in the library",
                ""
            };

            RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestSomeBooksInTheLibrary()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte'a 1 exemplu-\"   \"ISBN-13: 978-0-596-52068-7\"  74.679    ",
                " addbook  \"Abecedar - ?;\"  \"978 0 596 52068 7\"   342.67  20 ",
                " addbook  \"Crima si pedeapsa\" \"ISBN-10 0-596-52068-9\"  12.3783   5",
                "     getallbooks     ",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Book added successfully",
                "Book added successfully",
                "",
                "ISBN = ISBN-13: 978-0-596-52068-7, Name = Carte'a 1 exemplu-, Rent price = 74.68",
                "ISBN = 978 0 596 52068 7, Name = Abecedar - ?;, Rent price = 342.67",
                "ISBN = ISBN-10 0-596-52068-9, Name = Crima si pedeapsa, Rent price = 12.38",
                "",
                ""
            };

            RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]

        public void TestAvailableBooks()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte'a 1 exemplu-\"   \"ISBN-13: 978-0-596-52068-7\"  74.679    ",
                " addbook  \"Abecedar - ?;\"  \"978 0 596 52068 7\"   342.67  20 ",
                " addbook  \"Crima si pedeapsa\" \"ISBN-10 0-596-52068-9\"  12.3783   5",
                "     availablecopies   --name   \"Carte'a 1 exemplu-\"  ",
                "     availablecopies    --code  \"ISBN-10 0-596-52068-9\"  ",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Book added successfully",
                "Book added successfully",
                "Remaining copies of the book with name: \"Carte'a 1 exemplu-\" is 1",
                "Remaining copies of the book with ISBN: \"ISBN-10 0-596-52068-9\" is 5",
                ""
            };

            RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestBorrowBook()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte'a 1 exemplu-\"   \"ISBN-13: 978-0-596-52068-7\"  74.679    ",
                " addbook  \"Abecedar - ?;\"  \"978 0 596 52068 7\"   342.67  20 ",
                " addbook  \"Crima si pedeapsa\" \"ISBN-10 0-596-52068-9\"  12.3783   5",
                "     borrowbookto   --name   \"Carte'a 1 exemplu-\"   \"Adrian\"   16/10/2023 ",
                "     borrowbookto    --code  \"ISBN-10 0-596-52068-9\"  \"Georgeta\"   15/10/2023",
                "     availablecopies   --name   \"Carte'a 1 exemplu-\"  ",
                "     availablecopies    --code  \"ISBN-10 0-596-52068-9\"  ",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Book added successfully",
                "Book added successfully",
                "Book successfully borrowed",
                "Book successfully borrowed",
                "Remaining copies of the book with name: \"Carte'a 1 exemplu-\" is 0",
                "Remaining copies of the book with ISBN: \"ISBN-10 0-596-52068-9\" is 4",
                ""
            };

            RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestReturnBook()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte'a 1 exemplu-\"   \"ISBN-13: 978-0-596-52068-7\"  74.679    ",
                " addbook  \"Abecedar - ?;\"  \"978 0 596 52068 7\"   342.67  20 ",
                " addbook  \"Crima si pedeapsa\" \"ISBN-10 0-596-52068-9\"  12.3783   5",
                "     borrowbookto   --name   \"Carte'a 1 exemplu-\"   \"Adrian\"   16/10/2023 ",
                "     borrowbookto    --code  \"ISBN-10 0-596-52068-9\"  \"Georgeta\"   15/10/2023",
                "     availablecopies   --name   \"Carte'a 1 exemplu-\"  ",
                "     availablecopies    --code  \"ISBN-10 0-596-52068-9\"  ",
                "  returnbookfrom  --name   \"Carte'a 1 exemplu-\"   \"Adrian\"   2/11/2023 ",
                "  returnbookfrom  --code  \"ISBN-10 0-596-52068-9\"   \"Georgeta\"   25/10/2023 ",
                "     availablecopies   --name   \"Carte'a 1 exemplu-\"  ",
                "     availablecopies    --code  \"ISBN-10 0-596-52068-9\"  ",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Book added successfully",
                "Book added successfully",
                "Book successfully borrowed",
                "Book successfully borrowed",
                "Remaining copies of the book with name: \"Carte'a 1 exemplu-\" is 0",
                "Remaining copies of the book with ISBN: \"ISBN-10 0-596-52068-9\" is 4",
                "Reader Adrian has to pay 74.68 and additional penalty of 2.24 (3 days). TOTAL: 76.92",
                "Book successfully returned",
                "Reader Georgeta has to pay 12.38 without any penalties",
                "Book successfully returned",
                "Remaining copies of the book with name: \"Carte'a 1 exemplu-\" is 1",
                "Remaining copies of the book with ISBN: \"ISBN-10 0-596-52068-9\" is 5",
                ""
            };

            RunTest(arrayOfCommands, expectedOutputLines);
        }
    }
}