#ifndef EIGENE_FORMEN_H
#define EIGENE_FORMEN_H

#include "register.h"     // zieht Form + Factory mit
#include <QPolygonF>

// =====================================================================
// Klassenhierarchie fuer die Formen (siehe UML-Diagramm in ../uml):
//
//                     Form (abstrakt, Factory-Bibliothek)
//                    /                                   \
//         Vieleck (abstrakt, gerade Linien)      RundForm (abstrakt)
//        /      |        \        \                    |          \
//  Rechteck  Dreieck  Trapez  Parallelogramm        Ellipse     (Kreis <- Ellipse)
//     |                            |       \
//  Quadrat                        Raute   RegulieresPolygon
//                                             /        \
//                                        Sechseck      Stern
// =====================================================================

// Abstrakt: alle Formen, die aus einem Punktzug (Ecken) gezeichnet werden.
class Vieleck : public Form
{
protected:
    Vieleck(const QString name, const QString help, int x, int y);
    virtual QPolygonF points() const = 0;   // Ecken des Polygons
    void draw(QPainter &painter) override;
    // parseForm() bleibt offen -> Vieleck ist abstrakt
};

// Abstrakt: alle rund gezeichneten Formen (Ellipse als Grundform).
class RundForm : public Form
{
protected:
    double rx;   // waagerechter Radius
    double ry;   // senkrechter Radius
    RundForm(const QString name, const QString help, int x, int y,
             double rx, double ry);
    void draw(QPainter &painter) override;
};

// ---------------- Vieleck-Zweige ----------------

class Rechteck : public Vieleck
{
protected:
    double w, h;                 // Breite, Hoehe; (x,y) = oben links
    QPolygonF points() const override;
public:
    Rechteck(int x = 0, int y = 0, double w = 0, double h = 0);
    Form *parseForm(const QString &data) override;
};

// Quadrat ist nur ein Rechteck mit gleich langen Seiten.
class Quadrat : public Rechteck
{
public:
    explicit Quadrat(int x = 0, int y = 0, double seite = 0);
    Form *parseForm(const QString &data) override;
};

class Parallelogramm : public Vieleck
{
protected:
    double w, h, verschiebung;   // Schraege der oberen Kante
    QPolygonF points() const override;
public:
    Parallelogramm(int x = 0, int y = 0, double w = 0, double h = 0,
                   double verschiebung = 0);
    Form *parseForm(const QString &data) override;
};

// Raute: Parallelogramm mit vier gleich langen Seiten (ueber Diagonalen).
class Raute : public Parallelogramm
{
protected:
    double d1, d2;               // Diagonalen; (x,y) = Mittelpunkt
    QPolygonF points() const override;
public:
    Raute(int x = 0, int y = 0, double d1 = 0, double d2 = 0);
    Form *parseForm(const QString &data) override;
};

class Trapez : public Vieleck
{
protected:
    double oben, unten, h;       // beide parallelen Seiten, mittig buendig
    QPolygonF points() const override;
public:
    Trapez(int x = 0, int y = 0, double oben = 0, double unten = 0, double h = 0);
    Form *parseForm(const QString &data) override;
};

class Dreieck : public Vieleck
{
protected:
    double basis, h;
    QPolygonF points() const override;
public:
    Dreieck(int x = 0, int y = 0, double basis = 0, double h = 0);
    Form *parseForm(const QString &data) override;
};

// Regelmaessiges Polygon mit frei waehlbarer Eckenzahl.
class RegulieresPolygon : public Vieleck
{
protected:
    int ecken;
    double radius;               // Umkreisradius; (x,y) = Mittelpunkt
    QPolygonF points() const override;
public:
    RegulieresPolygon(int x = 0, int y = 0, int ecken = 0, double radius = 0);
    Form *parseForm(const QString &data) override;
};

class Sechseck : public RegulieresPolygon
{
public:
    explicit Sechseck(int x = 0, int y = 0, double radius = 0);
    Form *parseForm(const QString &data) override;
};

// Stern: regulaerer Stern mit aussen/innen Radien (2*spitzen Ecken).
class Stern : public RegulieresPolygon
{
protected:
    double radiusInnen;
    int spitzen;
    QPolygonF points() const override;
public:
    Stern(int x = 0, int y = 0, int spitzen = 0, double radius = 0,
          double radiusInnen = 0);
    Form *parseForm(const QString &data) override;
};

// ---------------- RundForm-Zweige ----------------

class Ellipse : public RundForm
{
public:
    Ellipse(int x = 0, int y = 0, double rx = 0, double ry = 0);
    Form *parseForm(const QString &data) override;
};

// Kreis ist nur eine Ellipse mit gleichen Radien.
class Kreis : public Ellipse
{
public:
    explicit Kreis(int x = 0, int y = 0, double radius = 0);
    Form *parseForm(const QString &data) override;
};

#endif // EIGENE_FORMEN_H
