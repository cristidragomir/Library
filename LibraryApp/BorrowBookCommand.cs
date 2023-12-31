﻿namespace Library
{
    public class BorrowBookCommand : BookCommand
    {
        private static readonly string commandPattern =
            @"^\s*(\w+)\s+\-\-(\w+)\s+""([\-\\\{\}\[\]\d()',?!;:\.\w\s]*)""\s+""([\-'\w\s]*)""\s+(\d\d?/\d\d?/\d\d\d\d)\s*$";
        
        public override void DoAction()
        {
            bool result = BorrowReturnBook(commandPattern, new BorrowBookAction());
            if (result)
            {
                Console.WriteLine("Book successfully borrowed");
            }
        }
    }
}