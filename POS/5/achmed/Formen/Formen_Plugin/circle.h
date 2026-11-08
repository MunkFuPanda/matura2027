#ifndef CIRCLE_H
#define CIRCLE_H

#include "register.h"

class PLUGIN Circle : public Form
{
protected:
    int radius;
public:
    Circle(int x = 0, int y = 0, int radius = 0);
    void draw(QPainter &painter) override;
    Form *parseForm(const QString &data) override;
};

#endif // CIRCLE_H
