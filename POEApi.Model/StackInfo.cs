namespace POEApi.Model
{
    public class StackInfo
    {
        public int Amount { get; set; }
        public int MaxSize { get; set; }

        internal StackInfo(int amount, int maxSize)
        {
            this.Amount = amount;
            this.MaxSize = maxSize;
        }
    }
}
