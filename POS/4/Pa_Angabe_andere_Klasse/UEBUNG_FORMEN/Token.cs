namespace UEBUNG_FORMEN
{
    internal class Token
    {
        internal enum TokenType
        {
            KEYWORD,
            OPEN_BRACE,
            CLOSE_BRACE,
            WORD,
            NUMBER,
            ERROR
        }

        public TokenType Type { get; set; }
        public string Value { get; set; }
        public int LineNumber { get; set; }
    }
}
