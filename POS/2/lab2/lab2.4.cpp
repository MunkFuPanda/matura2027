/* author: Markus Spitzer
 * date: 2023-10-05
 * file: Lab2.4.cpp
 * class: 2CHIF
 */

#include <iostream>

#include "validator.h"

int main(int argc, char **argv) {
    long width = lPromise(mkArg(argc, argv, 1), "Breite: ");
    long height = lPromise(mkArg(argc, argv, 2), "Höhe: ");

    for (int x = 0; x < height * (width + 1); x++) {
        std::cout << (x % (width + 1) == width ? "\n" : "*");
    }

    return 0;
}