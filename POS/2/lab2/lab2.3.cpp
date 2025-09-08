/* author: Markus Spitzer
 * date: 2023-10-05
 * file: Lab2.3.cpp
 * class: 2CHIF
 */

#include <iostream>

#include "validator.h"

int main(int argc, char** argv) {
    double start = dPromise(mkArg(argc, argv, 1), "Startkapital: ");
    double goal = dPromise(mkArg(argc, argv, 2), "Zielkapital: ");
    double multiplier = dPromise(mkArg(argc, argv, 3), "Zinssatz in %: ");

    int years;
    double current = start;

    for (years = 0; current < goal; years++) {
        current += current * (multiplier / 100);
    }

    std::cout << "Es dauert " << years << " Jahre\n";

    return 0;
}