#ifndef STACK_H
#define STACK_H

#include <stdexcept>

struct Stack
{
    static constexpr int MAX = 100;

    int werte[MAX];
    int anzahl = 0;

    void push(int zahl)
    {
        if (anzahl >= MAX)
            throw std::runtime_error("Stack ist voll");

        werte[anzahl] = zahl;
        anzahl++;
    }

    int pop()
    {
        if (anzahl == 0)
            throw std::runtime_error("Stack ist leer");

        anzahl--;
        return werte[anzahl];
    }

    bool leer() const
    {
        return anzahl == 0;
    }
};

#endif // STACK_H
