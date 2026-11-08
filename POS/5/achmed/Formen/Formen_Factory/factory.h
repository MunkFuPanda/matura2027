#ifndef FACTORY_H
#define FACTORY_H

#include <QObject>
#include <QMap>
#include <QStringList>
#include "form.h"

// Factory-Pattern: die App kennt nur "Form" und diese Factory.
// Plugins registrieren ihre Formen als Prototypen; createForm()
// erzeugt ueber parseForm() ein konkretes Objekt aus den Nutzereingaben.
class FORMEN_LIBRARY_SDK Factory : public QObject
{
    Q_OBJECT

private:
    QMap<QString, Form*> forms;        // Name -> Prototyp
    QStringList formNameList;
public:
    Factory(QObject* parent = nullptr);
    ~Factory() override;
    void registerForm(Form *form);
    Form* createForm(const QString &form, const QString &data);
    const QString &getHelpText(const QString &form);
    QStringList &getFormList();
};

#endif // FACTORY_H
