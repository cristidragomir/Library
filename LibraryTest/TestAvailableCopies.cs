using Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest
{
    [TestClass]
    public class TestAvailableCopies
    {
        [TestMethod]
        public void TestRequiredArgs()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  availablecopies   \"Carte'a 1: exemplu-\"     45.34  ",
                "  availablecopies   \"Carte'a 1: exemplu-\"  ",
                "availablecopies --option \"Carte'a 1: exemplu-\" ",
                "availablecopies ",
                "availablecopies --name",
                "availablecopies --code \"ISBN 11978-0-596-52068-7\" ",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Command is not given in proper format",
                "Command is not given in proper format",
                "Unknown option. Available options are: name, code",
                "Command is not given in proper format",
                "Command is not given in proper format",
                "Provided ISBN does not exist in the database",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestDuplicateNameDiffISBNs()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                " addbook \"Carte'a d-exemplu. Subtitlu\" \"0 512 52068 9\" 45.86 4 ",
                "addbook \"Carte'a d-exemplu. Subtitlu\" \"ISBN-10 0-596-52068-9\" 24.67 ",
                " availablecopies --name \"Carte'a d-exemplu. Subtitlu\"",
                " availablecopies --code \"ISBN-10 0-596-52068-9\"",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Book added successfully",
                "WARNING: Multiple books with the same name, but different ISBNs",
                "Books with the same name \"Carte'a d-exemplu. Subtitlu\" but different ISBNs:",
                "\t0 512 52068 9 : 4",
                "\tISBN-10 0-596-52068-9 : 1",
                "Remaining copies of the book with ISBN: \"ISBN-10 0-596-52068-9\" is 1",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestInexistentNameOrISBN()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                " addbook \"Carte'a d-exemplu. Subtitlu\" \"0 512 52068 9\" 45.86 4 ",
                "addbook \"Carte'a d-exemplu. Subtitlu\" \"ISBN-10 0-596-52068-9\" 24.67 ",
                " availablecopies --name \"Carte'a d-exemplu. SubtitluAALTUL\"",
                " availablecopies --code \"ISBN-10 0-596-52063-9\"",
                " availablecopies --code \"ISBN-10 0-596-52068-9\"",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Book added successfully",
                "Provided name of the book does not exist in the database",
                "Provided ISBN does not exist in the database",
                "Remaining copies of the book with ISBN: \"ISBN-10 0-596-52068-9\" is 1",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }
    }
}
