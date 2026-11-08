#include "eigene_formen.h"

#include <QtMath>
#include <QRectF>

// ---------- Hilfen ----------

// Komma-getrennte Eingabe in Doubles wandeln; prueft Anzahl und Gueltigkeit.
static bool parseZahlen(const QString &data, int anzahl, double werte[])
{
    const QStringList d = data.split(",", Qt::SkipEmptyParts);
    if (d.size() != anzahl)
        return false;
    for (int i = 0; i < anzahl; ++i) {
        bool ok = false;
        werte[i] = d.at(i).trimmed().toDouble(&ok);
        if (!ok)
            return false;
    }
    return true;
}

static QPolygonF Kreispunkte(double mx, double my, double n, double r,
                             double winkelanfang = -90.0)
{
    QPolygonF pts;
    for (int i = 0; i < int(n); ++i) {
        const double winkel = qDegreesToRadians(winkelanfang + i * 360.0 / n);
        pts << QPointF(mx + r * qCos(winkel), my + r * qSin(winkel));
    }
    return pts;
}

// ===================== Vieleck =====================

Vieleck::Vieleck(const QString name, const QString help, int x, int y)
    : Form(name, help, x, y)
{
}

void Vieleck::draw(QPainter &painter)
{
    painter.setPen(QPen(Qt::black, 2, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin));
    painter.setBrush(QColor(66, 133, 244, 70));   // dezente Flaeche
    painter.drawPolygon(points());
}

// ===================== RundForm =====================

RundForm::RundForm(const QString name, const QString help, int x, int y,
                   double rx, double ry)
    : Form(name, help, x, y), rx(rx), ry(ry)
{
}

void RundForm::draw(QPainter &painter)
{
    painter.setPen(QPen(Qt::black, 2, Qt::SolidLine, Qt::RoundCap));
    painter.setBrush(QColor(66, 133, 244, 70));
    painter.drawEllipse(QRectF(x - rx, y - ry, 2 * rx, 2 * ry));
}

// ===================== Rechteck / Quadrat =====================

Rechteck::Rechteck(int x, int y, double w, double h)
    : Vieleck("Rechteck", "Datenformat: x,y,breite,hoehe (x,y = oben links)",
              x, y), w(w), h(h)
{
}

QPolygonF Rechteck::points() const
{
    return QPolygonF()
        << QPointF(x, y) << QPointF(x + w, y)
        << QPointF(x + w, y + h) << QPointF(x, y + h);
}

Form *Rechteck::parseForm(const QString &data)
{
    double v[4];
    if (parseZahlen(data, 4, v) && v[2] > 0 && v[3] > 0)
        return new Rechteck(int(v[0]), int(v[1]), v[2], v[3]);
    return nullptr;
}

Quadrat::Quadrat(int x, int y, double seite)
    : Rechteck(x, y, seite, seite)          // Quadrat = Rechteck mit seite=seite
{
    name = "Quadrat";
    help = "Datenformat: x,y,seite (x,y = oben links)";
}

Form *Quadrat::parseForm(const QString &data)
{
    double v[3];
    if (parseZahlen(data, 3, v) && v[2] > 0)
        return new Quadrat(int(v[0]), int(v[1]), v[2]);
    return nullptr;
}

// ===================== Parallelogramm / Raute =====================

Parallelogramm::Parallelogramm(int x, int y, double w, double h, double verschiebung)
    : Vieleck("Parallelogramm",
              "Datenformat: x,y,breite,hoehe,verschiebung (x,y = unten links)",
              x, y), w(w), h(h), verschiebung(verschiebung)
{
}

QPolygonF Parallelogramm::points() const
{
    return QPolygonF()
        << QPointF(x, y) << QPointF(x + w, y)
        << QPointF(x + w + verschiebung, y - h)
        << QPointF(x + verschiebung, y - h);
}

Form *Parallelogramm::parseForm(const QString &data)
{
    double v[5];
    if (parseZahlen(data, 5, v) && v[2] > 0 && v[3] > 0 && v[4] != 0)
        return new Parallelogramm(int(v[0]), int(v[1]), v[2], v[3], v[4]);
    return nullptr;
}

Raute::Raute(int x, int y, double d1, double d2)
    : Parallelogramm(x, y), d1(d1), d2(d2)
{
    name = "Raute";
    help = "Datenformat: x,y,diagonale1,diagonale2 (x,y = Mittelpunkt)";
}

QPolygonF Raute::points() const
{
    return QPolygonF()
        << QPointF(x, y - d2 / 2) << QPointF(x + d1 / 2, y)
        << QPointF(x, y + d2 / 2) << QPointF(x - d1 / 2, y);
}

Form *Raute::parseForm(const QString &data)
{
    double v[4];
    if (parseZahlen(data, 4, v) && v[2] > 0 && v[3] > 0)
        return new Raute(int(v[0]), int(v[1]), v[2], v[3]);
    return nullptr;
}

// ===================== Trapez / Dreieck =====================

Trapez::Trapez(int x, int y, double oben, double unten, double h)
    : Vieleck("Trapez",
              "Datenformat: x,y,basisOben,basisUnten,hoehe (x,y = unten links)",
              x, y), oben(oben), unten(unten), h(h)
{
}

QPolygonF Trapez::points() const
{
    const double einzug = (unten - oben) / 2.0;   // obere Seite mittig
    return QPolygonF()
        << QPointF(x, y) << QPointF(x + unten, y)
        << QPointF(x + unten - einzug, y - h)
        << QPointF(x + einzug, y - h);
}

Form *Trapez::parseForm(const QString &data)
{
    double v[5];
    if (parseZahlen(data, 5, v) && v[2] > 0 && v[2] != v[3] && v[3] > 0 && v[4] > 0)
        return new Trapez(int(v[0]), int(v[1]), v[2], v[3], v[4]);
    return nullptr;
}

Dreieck::Dreieck(int x, int y, double basis, double h)
    : Vieleck("Dreieck",
              "Datenformat: x,y,basis,hoehe (x,y = unten links)", x, y),
      basis(basis), h(h)
{
}

QPolygonF Dreieck::points() const
{
    return QPolygonF()
        << QPointF(x, y) << QPointF(x + basis / 2.0, y - h)
        << QPointF(x + basis, y);
}

Form *Dreieck::parseForm(const QString &data)
{
    double v[4];
    if (parseZahlen(data, 4, v) && v[2] > 0 && v[3] > 0)
        return new Dreieck(int(v[0]), int(v[1]), v[2], v[3]);
    return nullptr;
}

// ===================== regelmaessige Polygone =====================

RegulieresPolygon::RegulieresPolygon(int x, int y, int ecken, double radius)
    : Vieleck("Polygon",
              "Datenformat: x,y,ecken,radius (x,y = Mittelpunkt, ecken >= 3)",
              x, y), ecken(ecken), radius(radius)
{
}

QPolygonF RegulieresPolygon::points() const
{
    return Kreispunkte(x, y, ecken, radius);
}

Form *RegulieresPolygon::parseForm(const QString &data)
{
    double v[4];
    if (parseZahlen(data, 4, v) && v[2] >= 3 && v[3] > 0)
        return new RegulieresPolygon(int(v[0]), int(v[1]), int(v[2]), v[3]);
    return nullptr;
}

Sechseck::Sechseck(int x, int y, double radius)
    : RegulieresPolygon(x, y, 6, radius)        // Sechseck = Polygon mit 6 Ecken
{
    name = "Sechseck";
    help = "Datenformat: x,y,radius (x,y = Mittelpunkt)";
}

Form *Sechseck::parseForm(const QString &data)
{
    double v[3];
    if (parseZahlen(data, 3, v) && v[2] > 0)
        return new Sechseck(int(v[0]), int(v[1]), v[2]);
    return nullptr;
}

Stern::Stern(int x, int y, int spitzen, double radius, double radiusInnen)
    : RegulieresPolygon(x, y, 2 * spitzen, radius),
      radiusInnen(radiusInnen), spitzen(spitzen)
{
    name = "Stern";
    help = "Datenformat: x,y,spitzen,radius,radiusInnen (x,y = Mittelpunkt)";
}

QPolygonF Stern::points() const
{
    // Spitzen und Innenpunkte im Wechsel
    QPolygonF pts = Kreispunkte(x, y, 2 * spitzen, radius);
    const QPolygonF innen =
        Kreispunkte(x, y, spitzen, radiusInnen, -90.0 + 180.0 / spitzen);
    QPolygonF stern;
    for (int i = 0; i < spitzen; ++i) {
        stern << pts.at(2 * i) << innen.at(i);
    }
    return stern;
}

Form *Stern::parseForm(const QString &data)
{
    double v[5];
    if (parseZahlen(data, 5, v) && v[2] >= 2 && v[3] > 0 && v[4] > 0 && v[4] < v[3])
        return new Stern(int(v[0]), int(v[1]), int(v[2]), v[3], v[4]);
    return nullptr;
}

// ===================== Ellipse / Kreis =====================

Ellipse::Ellipse(int x, int y, double rx, double ry)
    : RundForm("Ellipse",
               "Datenformat: x,y,radiusX,radiusY (x,y = Mittelpunkt)",
               x, y, rx, ry)
{
}

Form *Ellipse::parseForm(const QString &data)
{
    double v[4];
    if (parseZahlen(data, 4, v) && v[2] > 0 && v[3] > 0 && v[2] != v[3])
        return new Ellipse(int(v[0]), int(v[1]), v[2], v[3]);
    return nullptr;
}

Kreis::Kreis(int x, int y, double radius)
    : Ellipse(x, y, radius, radius)             // Kreis = Ellipse mit r=r
{
    name = "Kreis";
    help = "Datenformat: x,y,radius (x,y = Mittelpunkt)";
}

Form *Kreis::parseForm(const QString &data)
{
    double v[3];
    if (parseZahlen(data, 3, v) && v[2] > 0)
        return new Kreis(int(v[0]), int(v[1]), v[2]);
    return nullptr;
}
