#include "register.h"
#include "square.h"
#include "circle.h"
#include <iostream>

bool registerForm(Factory *factory)
{
    std::cout << "Squaring up..." << std::endl;
    factory->registerForm(new Square());

    std::cout << "Circling up..." << std::endl;
    factory->registerForm(new Circle());

    return true;
}
