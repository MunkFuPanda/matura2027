namespace Robotersteuerung.Parser
{
    public enum TokenType
    {
        MOVE,
        REPEAT,
        COLLECT,
        IF,
        UNTIL,
        LBRACE,
        RBRACE,
        UP,
        DOWN,
        LEFT,
        RIGHT,
        IS_A,
        OBSTACLE,
        NUMBER,
        LETTER,
        EOF,
        INVALID
    }

    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public Token(TokenType type, string value, int line, int column)
        {
            Type = type;
            Value = value;
            Line = line;
            Column = column;
        }

        public override string ToString()
        {
            return $"{Type}({Value}) at {Line}:{Column}";
        }
    }

    public class Lexer
    {
        private string _input;
        private int _position;
        private int _line;
        private int _column;

        public Lexer(string input)
        {
            _input = input;
            _position = 0;
            _line = 1;
            _column = 1;
        }

        private char Current => _position < _input.Length ? _input[_position] : '\0';
        private char Peek(int offset = 1) => _position + offset < _input.Length ? _input[_position + offset] : '\0';

        private void Advance()
        {
            if (Current == '\n')
            {
                _line++;
                _column = 1;
            }
            else
            {
                _column++;
            }
            _position++;
        }

        private void SkipWhitespace()
        {
            while (char.IsWhiteSpace(Current))
            {
                Advance();
            }
        }

        public Token NextToken()
        {
            SkipWhitespace();

            if (Current == '\0')
                return new Token(TokenType.EOF, "", _line, _column);

            int tokenLine = _line;
            int tokenColumn = _column;

            // Single character tokens
            if (Current == '{')
            {
                Advance();
                return new Token(TokenType.LBRACE, "{", tokenLine, tokenColumn);
            }

            if (Current == '}')
            {
                Advance();
                return new Token(TokenType.RBRACE, "}", tokenLine, tokenColumn);
            }

            // Keywords and identifiers
            if (char.IsLetter(Current))
            {
                string value = "";
                while (char.IsLetterOrDigit(Current))
                {
                    value += Current;
                    Advance();
                }

                return value.ToUpper() switch
                {
                    "MOVE" => new Token(TokenType.MOVE, value, tokenLine, tokenColumn),
                    "REPEAT" => new Token(TokenType.REPEAT, value, tokenLine, tokenColumn),
                    "COLLECT" => new Token(TokenType.COLLECT, value, tokenLine, tokenColumn),
                    "IF" => new Token(TokenType.IF, value, tokenLine, tokenColumn),
                    "UNTIL" => new Token(TokenType.UNTIL, value, tokenLine, tokenColumn),
                    "UP" => new Token(TokenType.UP, value, tokenLine, tokenColumn),
                    "DOWN" => new Token(TokenType.DOWN, value, tokenLine, tokenColumn),
                    "LEFT" => new Token(TokenType.LEFT, value, tokenLine, tokenColumn),
                    "RIGHT" => new Token(TokenType.RIGHT, value, tokenLine, tokenColumn),
                    "IS-A" => new Token(TokenType.IS_A, value, tokenLine, tokenColumn),
                    "IS_A" => new Token(TokenType.IS_A, value, tokenLine, tokenColumn),
                    "OBSTACLE" => new Token(TokenType.OBSTACLE, value, tokenLine, tokenColumn),
                    _ => new Token(TokenType.LETTER, value, tokenLine, tokenColumn),
                };
            }

            // Numbers
            if (char.IsDigit(Current))
            {
                string value = "";
                while (char.IsDigit(Current))
                {
                    value += Current;
                    Advance();
                }
                return new Token(TokenType.NUMBER, value, tokenLine, tokenColumn);
            }

            // Handle IS-A or IS_A specially
            if (Current == 'I' || Current == 'i')
            {
                string peek2 = Current.ToString() + Peek();
                if (char.ToUpper(peek2[0]) == 'I' && char.ToUpper(peek2[1]) == 'S')
                {
                    int tempPos = _position;
                    int tempCol = _column;
                    string word = "";
                    while (char.IsLetterOrDigit(Current) || Current == '-' || Current == '_')
                    {
                        word += Current;
                        Advance();
                    }
                    if (word.ToUpper() == "IS-A" || word.ToUpper() == "IS_A")
                    {
                        return new Token(TokenType.IS_A, word, tokenLine, tokenColumn);
                    }
                }
            }

            Advance();
            return new Token(TokenType.INVALID, Current.ToString(), tokenLine, tokenColumn);
        }

        public List<Token> Tokenize()
        {
            List<Token> tokens = new List<Token>();
            Token token;
            do
            {
                token = NextToken();
                tokens.Add(token);
            } while (token.Type != TokenType.EOF);

            return tokens;
        }
    }
}
