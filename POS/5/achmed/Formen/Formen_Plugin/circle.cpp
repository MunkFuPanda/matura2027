#include "circle.h"

Circle::Circle(int x, int y, int radius)
    : Form("Circle", "Datenformat: x,y,radius", x, y), radius(radius)
{

}

void Circle::draw(QPainter &painter)
{
    painter.setPen(QPen(Qt::black, 2, Qt::SolidLine, Qt::RoundCap));
    painter.drawEllipse(x - radius, y - radius, 2 * radius, 2 * radius);
}

Form *Circle::parseForm(const QString &data)
{
    QStringList d = data.split(",", Qt::SkipEmptyParts);
    if (d.size() == 3)
    {
        bool ok = false;
        const int xv = d[0].trimmed().toInt(&ok);
        const int yv = d[1].trimmed().toInt(&ok);
        const int rv = d[2].trimmed().toInt(&ok);
        if (ok && rv > 0)
            return new Circle(xv, yv, rv);
    }
    return nullptr;
}
