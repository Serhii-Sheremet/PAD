namespace PAD
{
    public class KeyValueData
    {
        public KeyValueData(string Text)
        {
            text = Text;
            itemId = 0;
        }

        public KeyValueData(string Text, int ItemId)
        {
            text = Text;
            itemId = ItemId;
        }

        public int ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public override string ToString()
        {
            return text;
        }

        protected string text;
        protected int itemId;
    }
}
