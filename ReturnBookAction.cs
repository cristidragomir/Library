﻿namespace Library
{
    internal class ReturnBookAction : BookAction
    {
        public override void DoAction()
        {
            if (CheckNullValues()) { return; }
            bookToModify.ReturnBook(reader, date);
        }
    }
}
