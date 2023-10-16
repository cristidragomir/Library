using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest
{
    [TestClass]
    public class TestAddBook
    {
        [TestMethod]
        public void TestMinimumNumberOfParams()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte'a 1 exemplu-\"   \"ISBN-13: 978-0-596-52068-7\"    ",
                "addbook   \"Carte'a 1 exemplu-\"",
                " addbook ",
                "  addbook   \"Carte'a 1 exemplu-\"   \"ISBN-13: 978-0-596-52068-7\"  45.34  ",
                "  addbook   \"Carte'a 1 exemplu-\"     45.34  ",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Command is not given in proper format",
                "Command is not given in proper format",
                "Command is not given in proper format",
                "Book added successfully",
                "Command is not given in proper format",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestInvalidBookNameOrISBN()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"\"   \"ISBN-13: 978-0-596-52068-7\"  74.679    ",
                "  addbook   \"\"\"\"   \"ISBN-13: 978-0-596-52068-7\"  74.679    ",
                "  addbook   \"Carte titlu-.subtitlu\"   \"ISBN-: 1223454665423423\"  74.679    ",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book name is empty",
                "Command is not given in proper format",
                "Invalid ISBN",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestInvalidPriceOrCopiesArg()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte titlu-.subtitlu\"   \"9780596520687\"  -12  -4    ", // regex not match
                "  addbook   \"Carte titlu-.subtitlu\"   \"9780596520687\"  0.89  5.7    ", // Copies error, regex not match
                "  addbook   \"Carte titlu-.subtitlu\"   \"9780596520687\"  0.89  5    ", //OK
                "  addbook   \"Carte titlu-.subtitlu\"   \"9780596520687\"  -12  5    ", // negative price
                "  addbook   \"Carte titlu-.subtitlu\"   \"9780596520687\"  -12  0    ", // negative price
                "  addbook   \"Carte titlu-.subtitlu\"   \"9780596521687\"  12  0    ", // 0 copies
                "  addbook   \"Carte titlu-.subtitlu\"   \"9780596520697\"  08346  0    ", //Price error
                "  addbook   \"Carte titlu-.subtitlu\"   \"9780596520697\"  0  0045363    ", // Copies error
                "  addbook   \"Carte titlu-.subtitlu\"   \"9780596520697\"  12  5    ", // OK
                "  addbook \"Carte titlu-.subtitlu\" \"\" 12 7",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Command is not given in proper format",
                "Command is not given in proper format",
                "Book added successfully",
                "Command is not given in proper format",
                "Command is not given in proper format",
                "Number of units provided not in the correct format",
                "Price is not provided in the correct format",
                "Number of units provided not in the correct format",
                "Book added successfully",
                "Invalid ISBN",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestAddDuplicateBook()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte titlu-.subtitlu\"   \"9780596520697\"  12  5    ",
                "  addbook   \"Carte titlu-.subtitlu\"   \"9780596520697\"  12  5    ",
                "  addbook   \"Carte titlu-.subtitluAltul\"   \"9780596520697\"  12  5    ",
                "  addbook   \"Carte titlu-.subtitlu\"   \"9780596620697\"  12  5    ",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Provided ISBN already exists in the database",
                "Provided ISBN already exists in the database",
                "Book added successfully",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }
    }
}
