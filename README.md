# Library

Created a Console Application in C# for managing books of a library and interaction with clients. LibraryTest contains different sets of unit tests meant to showcase the functionality of the piece of software and identify any potential issues or unexpected behaviour.



## Features

- Introduce books in the library based on their title, ISBN, rental price and number of copies
- Check the number of available copies of a book
- Query all available titles in the library
- Mechanism to borrow or return a book
- Interaction menu based on command line


## Run Locally

Clone the project

```bash
  git clone https://github.com/cristidragomir/Library
```

Go to the project directory

```bash
  cd Library/LibraryApp
```

Open Library.sln file, Visual Studio will be opened

### Running automated tests
In Solution Explorer will be 2 projects named: Library and LibraryTest

Right-click on LibraryTest and select Run Tests

### Hands-on experience
If you want to try your own commands, simply press the play button after opening the solution and a standard input console will show up. To syntax of the commands is explained in the following section.
## Usage/Examples

In the console the following commands are accepted:

### Adding a book to the library
```bash
addbook name ISBN rentalPrice [numberOfCopies] 
```
**name** and **ISBN** arguments must be surrounded by ".

[**numberOfCopies**] is an optional argument. Default number of copies for a book is 1.

### Retrieve information about all books in the library
```bash
getallbooks
```

### Check how many available copies are for a book
```bash
availablecopies --(name|code) identifier
```
**identifier** argument must be surrounded by ". 
From **(--name|code)** only one option must be selected.

If option is **--name**, then identifier represents the title of the book.

If option **--code**, then identifier represents the ISBN of the book.

*Note*: There might be the case where there are multiple books with the same name, but different ISBNs. In this case, to eliminate ambiguity, the option **--code** must be used.

### Borrow a copy of a book to a client
```bash
borrowbookto --(name|code) identifier readerID date
```
**identifier**, **readerID** arguments must be surrounded by ".

**readerID** is a string denoting the identity of an existent client.

**date** is of the format *dd/mm/yyyy*. Dates of the format *1/1/2000* are also valid.

[*availablecopies*](#check-how-many-available-copies-are-for-a-book) command explanations also apply for the current command.

### Return a copy of a book to the library
```bash
returnbookfrom --(name|code) identifier readerID date
```
Works in a similar fashion as [*borrowbookto*](#borrow-a-copy-of-a-book-to-a-client) command.

