namespace OmarFirstTask
{
    public class UndoNode<T>
    {
        public T Value;

        public UndoNode(T element)
        {
            this.Value = element;
        }
    }
}