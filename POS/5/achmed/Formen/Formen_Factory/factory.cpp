#include "factory.h"
#include <QObject>

Factory::Factory(QObject* parent):
 QObject(parent)
{
}

Factory::~Factory()
{
    qDeleteAll(forms);
    forms.clear();
}

void Factory::registerForm(Form *form)
{
    forms.insert(form->getName(), form);
    formNameList = forms.keys();
}

QStringList &Factory::getFormList()
{
    return formNameList;
}

// Zerlegt die Eingabe in der Prototyp-Klasse und liefert ein neues Objekt.
Form* Factory::createForm(const QString &form, const QString &data)
{
    if (forms.contains(form))
        return forms.value(form)->parseForm(data);
    return nullptr;
}

const QString &Factory::getHelpText(const QString &form)
{
    static const QString empty;
    if (forms.contains(form))
        return forms.value(form)->getHelp();
    return empty;
}
