using Robotersteuerung.Models;

namespace Robotersteuerung.Parser
{
    public class Parser
    {
        private List<Token> _tokens;
        private int _position;
        public List<string> Errors { get; private set; }

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _position = 0;
            Errors = new List<string>();
        }

        private Token Current => _position < _tokens.Count ? _tokens[_position] : new Token(TokenType.EOF, "", 0, 0);
        private Token Peek(int offset = 1) => _position + offset < _tokens.Count ? _tokens[_position + offset] : new Token(TokenType.EOF, "", 0, 0);

        private void Advance()
        {
            if (_position < _tokens.Count)
                _position++;
        }

        private bool Match(params TokenType[] types)
        {
            foreach (var type in types)
            {
                if (Current.Type == type)
                    return true;
            }
            return false;
        }

        private Token Consume(TokenType type, string errorMessage)
        {
            if (Current.Type == type)
            {
                Token token = Current;
                Advance();
                return token;
            }

            Errors.Add($"Zeile {Current.Line}, Spalte {Current.Column}: {errorMessage}");
            return null;
        }

        public Program Parse()
        {
            List<Command> commands = new List<Command>();

            while (Current.Type != TokenType.EOF)
            {
                if (Match(TokenType.INVALID))
                {
                    Errors.Add($"Zeile {Current.Line}, Spalte {Current.Column}: Ungültiges Token: '{Current.Value}'");
                    Advance();
                    continue;
                }

                Command command = ParseCommand();
                if (command != null)
                    commands.Add(command);
                else if (Errors.Count == 0 && Current.Type != TokenType.EOF)
                {
                    Errors.Add($"Zeile {Current.Line}, Spalte {Current.Column}: Unerwartetes Token: {Current.Type}");
                    Advance();
                }
            }

            return new Program(commands);
        }

        private Command ParseCommand()
        {
            if (Match(TokenType.MOVE))
            {
                Advance();
                return ParseMoveCommand();
            }
            else if (Match(TokenType.COLLECT))
            {
                Advance();
                return new CollectCommand();
            }
            else if (Match(TokenType.REPEAT))
            {
                Advance();
                return ParseRepeatCommand();
            }
            else if (Match(TokenType.IF))
            {
                Advance();
                return ParseIfCommand();
            }
            else if (Match(TokenType.UNTIL))
            {
                Advance();
                return ParseUntilCommand();
            }

            return null;
        }

        private MoveCommand ParseMoveCommand()
        {
            Direction direction = Direction.UP;

            if (Match(TokenType.UP))
            {
                direction = Direction.UP;
                Advance();
            }
            else if (Match(TokenType.DOWN))
            {
                direction = Direction.DOWN;
                Advance();
            }
            else if (Match(TokenType.LEFT))
            {
                direction = Direction.LEFT;
                Advance();
            }
            else if (Match(TokenType.RIGHT))
            {
                direction = Direction.RIGHT;
                Advance();
            }
            else
            {
                Errors.Add($"Zeile {Current.Line}, Spalte {Current.Column}: Richtung erwartet (UP, DOWN, LEFT, RIGHT)");
                Advance();
            }

            return new MoveCommand(direction);
        }

        private RepeatCommand ParseRepeatCommand()
        {
            if (!Match(TokenType.NUMBER))
            {
                Errors.Add($"Zeile {Current.Line}, Spalte {Current.Column}: Zahl erwartet nach REPEAT");
                return null;
            }

            int count = int.Parse(Current.Value);
            Advance();

            if (!Match(TokenType.LBRACE))
            {
                Errors.Add($"Zeile {Current.Line}, Spalte {Current.Column}: '{{' erwartet nach Zahl");
                return null;
            }

            Advance();
            List<Command> commands = ParseCommandBlock();

            if (!Match(TokenType.RBRACE))
            {
                Errors.Add($"Zeile {Current.Line}, Spalte {Current.Column}: '}}' erwartet zum Schließen des Blocks");
                return null;
            }

            Advance();
            return new RepeatCommand(count, commands);
        }

        private IfCommand ParseIfCommand()
        {
            Condition condition = ParseCondition();
            if (condition == null)
                return null;

            if (!Match(TokenType.LBRACE))
            {
                Errors.Add($"Zeile {Current.Line}, Spalte {Current.Column}: '{{' erwartet nach Bedingung");
                return null;
            }

            Advance();
            List<Command> commands = ParseCommandBlock();

            if (!Match(TokenType.RBRACE))
            {
                Errors.Add($"Zeile {Current.Line}, Spalte {Current.Column}: '}}' erwartet zum Schließen des Blocks");
                return null;
            }

            Advance();
            return new IfCommand(condition, commands);
        }

        private UntilCommand ParseUntilCommand()
        {
            Condition condition = ParseCondition();
            if (condition == null)
                return null;

            if (!Match(TokenType.LBRACE))
            {
                Errors.Add($"Zeile {Current.Line}, Spalte {Current.Column}: '{{' erwartet nach Bedingung");
                return null;
            }

            Advance();
            List<Command> commands = ParseCommandBlock();

            if (!Match(TokenType.RBRACE))
            {
                Errors.Add($"Zeile {Current.Line}, Spalte {Current.Column}: '}}' erwartet zum Schließen des Blocks");
                return null;
            }

            Advance();
            return new UntilCommand(condition, commands);
        }

        private Condition ParseCondition()
        {
            Direction direction = Direction.UP;

            if (Match(TokenType.UP))
            {
                direction = Direction.UP;
                Advance();
            }
            else if (Match(TokenType.DOWN))
            {
                direction = Direction.DOWN;
                Advance();
            }
            else if (Match(TokenType.LEFT))
            {
                direction = Direction.LEFT;
                Advance();
            }
            else if (Match(TokenType.RIGHT))
            {
                direction = Direction.RIGHT;
                Advance();
            }
            else
            {
                Errors.Add($"Zeile {Current.Line}, Spalte {Current.Column}: Richtung erwartet in Bedingung");
                return null;
            }

            if (!Match(TokenType.IS_A))
            {
                Errors.Add($"Zeile {Current.Line}, Spalte {Current.Column}: 'IS-A' erwartet");
                return null;
            }

            Advance();

            if (Match(TokenType.OBSTACLE))
            {
                Advance();
                return new Condition(direction, ConditionType.IS_OBSTACLE);
            }
            else if (Match(TokenType.LETTER))
            {
                char letter = Current.Value[0];
                Advance();
                return new Condition(direction, ConditionType.IS_LETTER, letter);
            }
            else
            {
                Errors.Add($"Zeile {Current.Line}, Spalte {Current.Column}: OBSTACLE oder Buchstabe erwartet nach 'IS-A'");
                return null;
            }
        }

        private List<Command> ParseCommandBlock()
        {
            List<Command> commands = new List<Command>();

            while (Current.Type != TokenType.RBRACE && Current.Type != TokenType.EOF)
            {
                Command command = ParseCommand();
                if (command != null)
                    commands.Add(command);
                else if (Current.Type != TokenType.RBRACE && Current.Type != TokenType.EOF)
                {
                    Errors.Add($"Zeile {Current.Line}, Spalte {Current.Column}: Unerwartetes Token: {Current.Type}");
                    Advance();
                }
            }

            return commands;
        }
    }
}
