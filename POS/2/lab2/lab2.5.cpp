/* author: Markus Spitzer
 * date: 2023-10-05
 * file: Lab2.5.cpp
 * class: 2CHIF
 */

#include <iostream>

#include "validator.h"

int main(int argc, char** argv) {
    long width = lPromise(mkArg(argc, argv, 1), "Breite: ");
    long height = lPromise(mkArg(argc, argv, 2), "Höhe: ");

    for (int h = 1; h <= height; h++) {
        for (int w = 1; w <= width; w++) {
            std::cout << (h == 1 || h == height || w == 1 || w == width ? "*"
                                                                        : " ");
        }
        std::cout << "\n";
    }

    return 0;
}