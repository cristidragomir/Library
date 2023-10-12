using Library;
using System.Xml.Linq;

namespace LibraryTest
{
    [TestClass]
    public class BasicTests
    {
        TextReader input;
        StringWriter output;

        private void SetConsoleIO(string inputStr)
        {
            input = new StringReader(inputStr);
            output = new StringWriter();

            Console.SetIn(input);
            Console.SetOut(output);
        }

        private void RunTest(List<string> arrayOfCommands, List<string> expectedOutputLines)
        {
            SetConsoleIO(string.Join("\n", arrayOfCommands));

            Program.Main(null);

            var result = output.ToString().Replace("\r", "");
            Assert.AreEqual(string.Join("\n", expectedOutputLines), result);

            input.Dispose();
            output.Dispose();
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
                "Book added successfully\n",
                "ISBN = ISBN-13: 978-0-596-52068-7, Name = Carte'a 1 exemplu-, Rent price = 74.679",
                "ISBN = 978 0 596 52068 7, Name = Abecedar - ?;, Rent price = 342.67",
                "ISBN = ISBN-10 0-596-52068-9, Name = Crima si pedeapsa, Rent price = 12.3783",
                "\n"
            };

            RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]

        public void 
    }
}