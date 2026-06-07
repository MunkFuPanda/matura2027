using System;
using Painter;
using PainterPA;

public class LineExpression : MoveExpression {
    internal override void Run(PainterControl painter) {
        painter.Line(X, Y);
    }
}
