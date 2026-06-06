using System.Collections.Generic;

namespace MosaikWerkstatt.Language
{
    // ---------------------------------------------------------------------
    // AST + INTERPRETER-PATTERN  (Teilaufgabe "Klassen nach Interpreter-Pattern")
    //
    // Kern-Idee des Interpreter-Patterns:
    //  - Jede Grammatik-Regel wird zu EINER Klasse.
    //  - Jede Klasse hat eine Methode Interpret(Context), die ihren Teil
    //    der Ausfuehrung erledigt.
    //  - Verschachtelte Konstrukte (REPEAT/WHILE/IF/Block) rufen
    //    Interpret(...) ihrer Kinder auf -> der Baum "fuehrt sich selbst aus".
    //
    // Anpassen bei neuer Sprache: Pro neuer Anweisung eine neue Klasse mit
    // Interpret(...). Sonst aendert sich hier nichts.
    // ---------------------------------------------------------------------

    public interface IStatement
    {
        void Interpret(Context ctx);
    }

    // --- Block: { stmt* } --------------------------------------------------
    public class Block : IStatement
    {
        public List<IStatement> Statements { get; } = new List<IStatement>();

        public void Interpret(Context ctx)
        {
            foreach (var s in Statements)
                s.Interpret(ctx);
        }
    }

    // --- SET COLOR <farbe> -------------------------------------------------
    public class SetColorStatement : IStatement
    {
        private readonly string _color;
        public SetColorStatement(string color) { _color = color; }

        public void Interpret(Context ctx)
        {
            ctx.SetColor(_color);
        }
    }

    // --- MOVE <richtung> [anzahl] -----------------------------------------
    public class MoveStatement : IStatement
    {
        private readonly string _direction;
        private readonly int _steps;
        public MoveStatement(string direction, int steps)
        {
            _direction = direction;
            _steps = steps;
        }

        public void Interpret(Context ctx)
        {
            for (int i = 0; i < _steps; i++)
                ctx.Move(_direction);
        }
    }

    // --- REPEAT <n> { block } ---------------------------------------------
    public class RepeatStatement : IStatement
    {
        private readonly int _count;
        private readonly Block _body;
        public RepeatStatement(int count, Block body)
        {
            _count = count;
            _body = body;
        }

        public void Interpret(Context ctx)
        {
            for (int i = 0; i < _count; i++)
                _body.Interpret(ctx);
        }
    }

    // --- WHILE CAN <richtung> { block } -----------------------------------
    public class WhileStatement : IStatement
    {
        private readonly string _direction;
        private readonly Block _body;
        public WhileStatement(string direction, Block body)
        {
            _direction = direction;
            _body = body;
        }

        public void Interpret(Context ctx)
        {
            // Sicherheitslimit gegen Endlosschleifen (defensive Programmierung).
            int guard = 0;
            while (ctx.CanMove(_direction))
            {
                _body.Interpret(ctx);
                if (++guard > 100000)
                    throw new RuntimeException("Abbruch: WHILE laeuft zu lange (Endlosschleife?).");
            }
        }
    }

    // --- IF CAN <richtung> { block } --------------------------------------
    public class IfStatement : IStatement
    {
        private readonly string _direction;
        private readonly Block _body;
        public IfStatement(string direction, Block body)
        {
            _direction = direction;
            _body = body;
        }

        public void Interpret(Context ctx)
        {
            if (ctx.CanMove(_direction))
                _body.Interpret(ctx);
        }
    }
}
