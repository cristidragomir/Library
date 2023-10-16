namespace Library
{
    public class BorrowBookAction : BookAction
    {
        public override void DoAction()
        {
            if (CheckNullValues() == false) { result = false; return; }
            result = bookToModify.BorrowBook(reader, date);
        }
    }
}
