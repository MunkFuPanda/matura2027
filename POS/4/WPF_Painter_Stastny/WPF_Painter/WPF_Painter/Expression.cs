namespace WPF_Painter
{
    internal abstract class Expression
    {
        internal static List<String> Errors = new List<string>();
        internal abstract void Parse(List<Token> tokens);
        internal abstract void Interpret(PainterContext context);
    }
}
