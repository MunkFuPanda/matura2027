namespace WPF_Painter
{
    internal class Token
    {
        public enum TokenType
        {
            KEYWORD,
            OPEN_BRACE,
            CLOSE_BRACE,
            LETTER,
            NUMBER,
            ERROR
        }
        internal TokenType Type { get; set; }
        internal string Value { get; set; }
    }
}
