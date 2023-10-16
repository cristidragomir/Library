using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LibraryTest
{
    [TestClass]
    public class TestBorrow
    {
        [TestMethod]
        public void TestCommandArgsCorrectness()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  borrowbookto   \"Carte'a 1: exemplu-\"     \"\"  99/12/2027 ",
                "  borrowbookto  --name  \"Carte'a 1: exemplu-\"     \"David\"  99/12/2027 ",
                "  borrowbookto   --code  \"\"  99/12/2027 ",
                "  borrowbookto   --option  \"Carte'a 1: exemplu-\" \"David-Alexandru\"  99/12/2027 ",
                " borrowbookto ",
                "borrowbookto 1/1/2000",
                "borrowbookto \"David\"  2/2/20",
                "  borrowbookto  --name  \"\"     \"David\"  9/12/2027 ",
                "  borrowbookto  --name  \"Carte'a 1: exemplu-\"     \"\"  9/12/2027 ",
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
        public void TestInexistentNameOrISBN()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte'a 1: exemplu-\"     \"9780596520687\"  56 ",
                "  addbook   \"Carte'a 1: exemplu2-\"     \"9780596521687\"  78.23 ",
                "  borrowbookto  --name  \"Carte'a 2: exemplu-\"     \"Claudiu\"  9/12/2027 ",
                "  borrowbookto   --code  \"9780596521697\" \"Claudiu\" 9/12/2027 ",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Book added successfully",
                "Provided name of the book does not exist in the database",
                "Provided ISBN does not exist in the database",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestNameAmbiguity()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte'a 1: exemplu-\"     \"9780596520687\"  56 ",
                "  addbook   \"Carte'a 1: exemplu-\"     \"9780596521687\"  78.23 ",
                "  borrowbookto  --name  \"Carte'a 1: exemplu-\"     \"Claudiu\"  9/12/2027 ",
                "  borrowbookto  --code  \"9780596521687\"     \"Claudiu\"  9/12/2027 ",
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
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestUnavailableBook()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte'a 1: exempluA-\"     \"9780596520687\"  56 2",
                "  addbook   \"Carte'a 1: exempluB-\"     \"9780596521687\"  78.23 ",
                "  borrowbookto  --name  \"Carte'a 1: exempluA-\"     \"Claudiu\"  9/12/2027 ",
                "  borrowbookto  --code  \"9780596520687\"     \"Bogdan\"  9/12/2027 ",
                "  borrowbookto  --name  \"Carte'a 1: exempluA-\"     \"Georgeta\"  9/12/2027 ",
                "  borrowbookto  --code  \"9780596520687\"     \"Mihaela\"  9/12/2027 ",
                "  borrowbookto  --name  \"Carte'a 1: exempluB-\"     \"Alex\"  9/12/2027 ",
                "  borrowbookto  --name  \"Carte'a 1: exempluB-\"     \"Adrian\"  9/12/2027 ",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Book added successfully",
                "Book successfully borrowed",
                "Book successfully borrowed",
                "There are no more copies available for this book + [ISBN = 9780596520687, Name = Carte'a 1: exempluA-, Rent price = 56]",
                "There are no more copies available for this book + [ISBN = 9780596520687, Name = Carte'a 1: exempluA-, Rent price = 56]",
                "Book successfully borrowed",
                "There are no more copies available for this book + [ISBN = 9780596521687, Name = Carte'a 1: exempluB-, Rent price = 78.23]",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }

        [TestMethod]
        public void TestDoubleBorrowSamePerson()
        {
            List<string> arrayOfCommands = new List<string>()
            {
                "  addbook   \"Carte'a 1: exempluA-\"     \"9780596520687\"  56 2",
                "  addbook   \"Carte'a 1: exempluB-\"     \"9780596521687\"  78.23 ",
                "  borrowbookto  --name  \"Carte'a 1: exempluA-\"     \"Claudiu\"  9/12/2027 ",
                "  borrowbookto  --code  \"9780596520687\"     \"Claudiu\"  10/12/2027 ",
                "  borrowbookto  --name  \"Carte'a 1: exempluB-\"     \"Georgeta\"  9/12/2027 ",
                "  borrowbookto  --code  \"9780596521687\"     \"Georgeta\"  9/12/2027 ",
                "  borrowbookto  --name  \"Carte'a 1: exempluB-\"     \"Georgeta\"  9/12/2027 ",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Book added successfully",
                "Book successfully borrowed",
                "The reader has already borrowed a copy of this book",
                "Book successfully borrowed",
                "The reader has already borrowed a copy of this book",
                "The reader has already borrowed a copy of this book",
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
                "  borrowbookto  --name  \"Carte'a 1: exempluA-\"     \"Clauiu\"  9/12/2027 ",
                "  borrowbookto  --name  \"Carte'a 1: exempluA-\"     \"Claudiu\"  9/12/2027 ",
                "  borrowbookto  --name  \"Carte'a 1: exempluB-\"     \"Georget\"  9/12/2027 ",
                "  borrowbookto  --code  \"9780596521687\"     \"Georgeta\"  9/12/2027 ",
                "  exit   \n"
            };

            List<string> expectedOutputLines = new List<string>()
            {
                "Book added successfully",
                "Book added successfully",
                "Reader does not exist in the database",
                "Book successfully borrowed",
                "Reader does not exist in the database",
                "Book successfully borrowed",
                ""
            };

            BasicTests.RunTest(arrayOfCommands, expectedOutputLines);
        }
    }
}
