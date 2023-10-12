namespace Library
{
    public class BorrowBookAction : BookAction
    {
        public override void DoAction()
        {
            if (CheckNullValues()) { return; }
            bookToModify.BorrowBook(reader, date);
        }
    }
}
