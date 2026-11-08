#include "register.h"
#include "eigene_formen.h"
#include <iostream>

bool registerForm(Factory *factory)
{
    std::cout << "Registriere eigene Formen..." << std::endl;

    factory->registerForm(new Rechteck());
    factory->registerForm(new Quadrat());
    factory->registerForm(new Kreis());
    factory->registerForm(new Ellipse());
    factory->registerForm(new Trapez());
    factory->registerForm(new Parallelogramm());
    factory->registerForm(new Raute());
    factory->registerForm(new Dreieck());
    factory->registerForm(new RegulieresPolygon());
    factory->registerForm(new Sechseck());
    factory->registerForm(new Stern());

    return true;
}
