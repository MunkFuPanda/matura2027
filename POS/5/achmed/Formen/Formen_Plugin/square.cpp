#include "square.h"

Square::Square(int x, int y, int width)
    : Form("Square", "Datenformat: x,y,breite", x, y), width(width)
{

}

void Square::draw(QPainter &painter)
{
    painter.setPen(QPen(Qt::black, 2, Qt::SolidLine, Qt::RoundCap));
    painter.drawRect(x, y, width, width);
}

Form *Square::parseForm(const QString &data)
{
    QStringList d = data.split(",", Qt::SkipEmptyParts);
    if (d.size() == 3)
    {
        bool ok = false;
        const int xv = d[0].trimmed().toInt(&ok);
        const int yv = d[1].trimmed().toInt(&ok);
        const int wv = d[2].trimmed().toInt(&ok);
        if (ok && wv > 0)
            return new Square(xv, yv, wv);
    }
    return nullptr;
}
