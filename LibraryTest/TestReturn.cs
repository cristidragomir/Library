using Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LibraryTest
{
    [TestClass]
    public class TestReturn
    {
        [TestMethod]
        public void TestCommandArgsCorrectness()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  returnbookfrom   \"Carte'a 1: exemplu-\"     \"\"  99/12/2027 ",
                "  returnbookfrom  --name  \"Carte'a 1: exemplu-\"     \"David\"  99/12/2027 ",
                "  returnbookfrom   --code  \"\"  99/12/2027 ",
                "  returnbookfrom   --option  \"Carte'a 1: exemplu-\" \"David-Alexandru\"  99/12/2027 ",
                " returnbookfrom ",
                "returnbookfrom 1/1/2000",
                "returnbookfrom \"David\"  2/2/20",
                "  returnbookfrom  --name  \"\"     \"David\"  9/12/2027 ",
                "  returnbookfrom  --name  \"Carte'a 1: exemplu-\"     \"\"  9/12/2027 ",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Command is not given in proper format",
                "Provided date is invalid",
                "Command is not given in proper format",
                "Provided date is invalid",
                "Command is not given in proper format",
                "Command is not given in proper format",
                "Command is not given in proper format",
                "Given argument is an empty string",
                "Given argument is an empty string",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestInexistentReader()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte'a 1: exempluA-\"     \"9780596520687\"  56 2",
                "  addbook   \"Carte'a 1: exempluB-\"     \"9780596521687\"  78.23 ",
                "  borrowbookto  --name  \"Carte'a 1: exempluA-\"     \"Claudiu\"  9/12/2027 ",
                "  returnbookfrom  --name  \"Carte'a 1: exempluA-\"     \"Clauiu\"  19/12/2027 ",
                "  returnbookfrom  --name  \"Carte'a 1: exempluA-\"     \"Claudiu\"  19/12/2027 ",
                "  borrowbookto  --name  \"Carte'a 1: exempluB-\"     \"Georgeta\"  9/12/2027 ",
                "  returnbookfrom  --name  \"Carte'a 1: exempluB-\"     \"Georget\"  19/12/2027 ",
                "  returnbookfrom  --code  \"9780596521687\"     \"Georgeta\"  19/12/2027 ",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Book added successfully",
                "Book successfully borrowed",
                "Reader does not exist in the database",
                "Reader Claudiu has to pay 56 without any penalties",
                "Book successfully returned",
                "Book successfully borrowed",
                "Reader does not exist in the database",
                "Reader Georgeta has to pay 78.23 without any penalties",
                "Book successfully returned",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestInexistentBookNameOrISBN()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte'a 1: exempluA-\"     \"9780596520687\"  56 2",
                "  addbook   \"Carte'a 1: exempluB-\"     \"9780596521687\"  78.23 ",
                "  borrowbookto  --name  \"Carte'a 1: exempluA-\"     \"Claudiu\"  9/12/2027 ",
                "  returnbookfrom  --name  \"Carte'a 1: exempluC-\"     \"Claudiu\"  19/12/2027 ",
                "  returnbookfrom  --code  \"9780596520697\"     \"Claudiu\"  19/12/2027 ",
                "  returnbookfrom  --code  \"9780596520687\"     \"Claudiu\"  19/12/2027 ",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Book added successfully",
                "Book successfully borrowed",
                "Provided name of the book does not exist in the database",
                "Provided ISBN does not exist in the database",
                "Reader Claudiu has to pay 56 without any penalties",
                "Book successfully returned",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestInvalidReturn()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte'a 1: exempluA-\"     \"9780596520687\"  56 2",
                "  addbook   \"Carte'a 1: exempluB-\"     \"9780596521687\"  78.23 ",
                "  borrowbookto  --name  \"Carte'a 1: exempluA-\"     \"Claudiu\"  9/12/2027 ",
                " returnbookfrom --name \"Carte'a 1: exempluA-\" \"Georgeta\" 19/12/2027",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Book added successfully",
                "Book successfully borrowed",
                "WARNING: This reader is not registered as a borrower of this book",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestSameNameDiffISBNs()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte'a 1: exemplu-\"     \"9780596520687\"  56 2",
                "  addbook   \"Carte'a 1: exemplu-\"     \"9780596521687\"  78.23 ",
                "  borrowbookto  --name  \"Carte'a 1: exemplu-\"     \"Claudiu\"  9/12/2027 ",
                " borrowbookto  --code  \"9780596520687\"     \"Claudiu\"  9/12/2027 ",
                " returnbookfrom --name \"Carte'a 1: exemplu-\" \"Claudiu\" 19/12/2027",
                " returnbookfrom --code \"9780596520687\" \"Claudiu\" 19/12/2027",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Book added successfully",
                "WARNING: Multiple books with the same name, but different ISBNs",
                "\t9780596520687",
                "\t9780596521687",
                "Please specify the ISBN",
                "Book successfully borrowed",
                "WARNING: Multiple books with the same name, but different ISBNs",
                "\t9780596520687",
                "\t9780596521687",
                "Please specify the ISBN",
                "Reader Claudiu has to pay 56 without any penalties",
                "Book successfully returned",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestAmntToPay()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte'a 1: exempluA-\"     \"9780596520687\"  56 2",
                "  addbook   \"Carte'a 1: exempluB-\"     \"9780596521687\"  78.23 ",
                "  borrowbookto  --name  \"Carte'a 1: exempluA-\"     \"Claudiu\"  9/12/2027 ",
                " borrowbookto  --code  \"9780596521687\"     \"Georgeta\"  9/12/2027 ",
                " returnbookfrom --name \"Carte'a 1: exempluA-\" \"Claudiu\" 19/12/2027",
                " returnbookfrom --name \"Carte'a 1: exempluB-\" \"Georgeta\" 28/12/2027",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Book added successfully",
                "Book successfully borrowed",
                "Book successfully borrowed",
                "Reader Claudiu has to pay 56 without any penalties",
                "Book successfully returned",
                "Reader Georgeta has to pay 78.23 and additional penalty of 3.91 (5 days). TOTAL: 82.14",
                "Book successfully returned",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }
    }
}
