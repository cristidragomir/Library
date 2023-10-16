namespace Library
{
    public class ReturnBookAction : BookAction
    {
        public override void DoAction()
        {
            if (CheckNullValues() == false) { result = false; return; }
            result = bookToModify.ReturnBook(reader, date);
        }
    }
}
