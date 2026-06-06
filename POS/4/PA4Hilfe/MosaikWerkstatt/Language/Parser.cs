using System.Collections.Generic;

namespace MosaikWerkstatt.Language
{
    // ---------------------------------------------------------------------
    // PARSER  (rekursiver Abstieg)  -- Teilaufgaben "Parser" + "Fehler"
    //
    // Baut aus der Token-Liste den AST (Baum aus IStatement-Objekten).
    // JEDE ABNF-Regel = EINE ParseXxx()-Methode. Das ist der ganze Trick:
    //
    //   program   = *statement
    //   statement = setcolor / move / repeat / while / if
    //   setcolor  = "SET" "COLOR" color
    //   move      = "MOVE" direction [number]
    //   repeat    = "REPEAT" number block
    //   while     = "WHILE" "CAN" direction block
    //   if        = "IF" "CAN" direction block
    //   block     = "{" *statement "}"
    //
    // Helfer Peek/Next/Expect kapseln den Token-Strom. Diese drei sind
    // SPRACHUNABHAENGIG -> bei neuer Angabe nur ParseStatement + die
    // gueltigen Mengen (Directions/Colors) anpassen.
    // ---------------------------------------------------------------------
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _pos;

        // Gueltige Werte -> hier zentral pflegen.
        private static readonly HashSet<string> Directions =
            new HashSet<string> { "UP", "DOWN", "LEFT", "RIGHT" };
        private static readonly HashSet<string> Colors =
            new HashSet<string> { "Black", "Red", "Green", "Blue", "Yellow", "White" };

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _pos = 0;
        }

        // --- Token-Strom-Helfer -------------------------------------------
        private Token Peek() { return _tokens[_pos]; }

        private Token Next() { return _tokens[_pos++]; }

        // Erwartet einen bestimmten Token-Typ; sonst aussagekraeftiger Fehler.
        private Token Expect(TokenType type, string what)
        {
            Token t = Peek();
            if (t.Type != type)
                throw new ParseException(
                    "Erwartet " + what + ", gefunden '" + t.Text + "'", t.Line, t.Column);
            return Next();
        }

        // Erwartet ein bestimmtes Schluesselwort (ein WORD mit festem Text).
        private Token ExpectWord(string keyword)
        {
            Token t = Peek();
            if (t.Type != TokenType.Word || t.Text != keyword)
                throw new ParseException(
                    "Erwartet '" + keyword + "', gefunden '" + t.Text + "'", t.Line, t.Column);
            return Next();
        }

        // --- Einstieg: program = *statement -------------------------------
        public Block ParseProgram()
        {
            var program = new Block();
            while (Peek().Type != TokenType.End)
                program.Statements.Add(ParseStatement());
            return program;
        }

        // --- statement = Fallunterscheidung ueber das erste Schluesselwort -
        private IStatement ParseStatement()
        {
            Token t = Peek();
            if (t.Type != TokenType.Word)
                throw new ParseException(
                    "Anweisung erwartet (SET/MOVE/REPEAT/WHILE/IF), gefunden '" + t.Text + "'",
                    t.Line, t.Column);

            switch (t.Text)
            {
                case "SET": return ParseSetColor();
                case "MOVE": return ParseMove();
                case "REPEAT": return ParseRepeat();
                case "WHILE": return ParseWhile();
                case "IF": return ParseIf();
                default:
                    throw new ParseException(
                        "Unbekanntes Schluesselwort '" + t.Text + "'", t.Line, t.Column);
            }
        }

        // setcolor = "SET" "COLOR" color
        private IStatement ParseSetColor()
        {
            ExpectWord("SET");
            ExpectWord("COLOR");
            Token c = Expect(TokenType.Word, "eine Farbe");
            if (!Colors.Contains(c.Text))
                throw new ParseException(
                    "Ungueltige Farbe '" + c.Text + "'. Erlaubt: " + string.Join(", ", Colors),
                    c.Line, c.Column);
            return new SetColorStatement(c.Text);
        }

        // move = "MOVE" direction [number]
        private IStatement ParseMove()
        {
            ExpectWord("MOVE");
            string dir = ParseDirection();
            int steps = 1; // Standard: ein Schritt
            if (Peek().Type == TokenType.Number)
                steps = int.Parse(Next().Text);
            return new MoveStatement(dir, steps);
        }

        // repeat = "REPEAT" number block
        private IStatement ParseRepeat()
        {
            ExpectWord("REPEAT");
            Token n = Expect(TokenType.Number, "eine Zahl");
            Block body = ParseBlock();
            return new RepeatStatement(int.Parse(n.Text), body);
        }

        // while = "WHILE" "CAN" direction block
        private IStatement ParseWhile()
        {
            ExpectWord("WHILE");
            ExpectWord("CAN");
            string dir = ParseDirection();
            Block body = ParseBlock();
            return new WhileStatement(dir, body);
        }

        // if = "IF" "CAN" direction block
        private IStatement ParseIf()
        {
            ExpectWord("IF");
            ExpectWord("CAN");
            string dir = ParseDirection();
            Block body = ParseBlock();
            return new IfStatement(dir, body);
        }

        // block = "{" *statement "}"
        private Block ParseBlock()
        {
            Expect(TokenType.LBrace, "'{'");
            var block = new Block();
            while (Peek().Type != TokenType.RBrace)
            {
                // Schutz: Datei zu Ende, aber '}' fehlt -> klare Meldung.
                if (Peek().Type == TokenType.End)
                {
                    Token end = Peek();
                    throw new ParseException("Erwartet '}', aber Programmende erreicht "
                        + "(fehlende schliessende Klammer?)", end.Line, end.Column);
                }
                block.Statements.Add(ParseStatement());
            }
            Expect(TokenType.RBrace, "'}'");
            return block;
        }

        // direction (eigene Regel, da an mehreren Stellen gebraucht)
        private string ParseDirection()
        {
            Token d = Expect(TokenType.Word, "eine Richtung (UP/DOWN/LEFT/RIGHT)");
            if (!Directions.Contains(d.Text))
                throw new ParseException(
                    "Ungueltige Richtung '" + d.Text + "'. Erlaubt: " + string.Join(", ", Directions),
                    d.Line, d.Column);
            return d.Text;
        }
    }
}
