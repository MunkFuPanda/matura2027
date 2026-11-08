#ifndef FORM_H
#define FORM_H

#include <QObject>
#include <QPainter>

#include <QtCore/qglobal.h>

#if defined(FORMEN_LIBRARY)
#  define FORMEN_LIBRARY_SDK Q_DECL_EXPORT
#else
#  define FORMEN_LIBRARY_SDK Q_DECL_IMPORT
#endif

// Abstrakte Basisklasse fuer alle Formen.
class FORMEN_LIBRARY_SDK Form
{
protected:
    QString name;
    QString help;
    int x;
    int y;
public:
    Form(const QString name, const QString help, int x, int y);
    virtual ~Form();
    virtual void draw(QPainter &painter) = 0;
    virtual Form *parseForm(const QString &data) = 0;
    const QString &getName() { return name; }
    const QString &getHelp() { return help; }
};

#endif // FORM_H
