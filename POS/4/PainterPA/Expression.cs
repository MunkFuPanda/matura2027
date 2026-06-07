using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Painter;
using PainterPA;

namespace PainterPA {
    public abstract class Expression {
        internal static List<String> Errors { get; set; } = [];
        internal abstract void Parse(List<Token> tokenList);
        internal abstract void Run(PainterControl painter);

        internal static readonly Dictionary<string, Func<Expression>> ExpressionFactories = new() {
            ["MOVE"] = () => new MoveExpression(),
            ["COLOR"] = () => new ColorExpression(),
            ["LINE"] = () => new LineExpression(),
            ["FOR"] = () => new ForExpression(),
            ["FORM"] = () => new FormExpression()
        };
    }
}