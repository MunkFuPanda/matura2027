using System.Collections.Generic;

namespace MosaikWerkstatt.Language
{
    // ---------------------------------------------------------------------
    // FASSADE: bindet Lexer -> Parser -> Ausfuehrung zusammen.
    // Das ist der EINZIGE Typ, den die GUI kennen muss. Komplett GUI-frei.
    //
    // Verwendung in der GUI (Code-Behind):
    //
    //   var interp = new MosaikInterpreter(rows: 9, cols: 9);
    //   List<Frame> frames = interp.Run(sourceText);   // wirft ParseException / RuntimeException
    //   // -> frames danach getaktet auf dem Feld abspielen
    // ---------------------------------------------------------------------
    public class MosaikInterpreter
    {
        private readonly int _rows;
        private readonly int _cols;

        public MosaikInterpreter(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;
        }

        // Nur parsen (z.B. um Syntax zu pruefen, ohne auszufuehren).
        public Block Parse(string source)
        {
            var lexer = new Lexer();
            List<Token> tokens = lexer.Tokenize(source);  // kann ParseException werfen
            var parser = new Parser(tokens);
            return parser.ParseProgram();                 // kann ParseException werfen
        }

        // Parsen + ausfuehren. Liefert die Frames fuer die Animation zurueck.
        public List<MFrame> Run(string source)
        {
            Block program = Parse(source);
            var ctx = new Context(_rows, _cols);
            program.Interpret(ctx);   // kann RuntimeException werfen
            return ctx.Frames;
        }
    }
}
