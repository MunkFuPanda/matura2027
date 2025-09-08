/* author: Markus Spitzer
 * date: 2023-10-05
 * file: Lab2.2.cpp
 * class: 2CHIF
 */

#include <iostream>

#include "validator.h"

int main(int argc, char** argv) {
    long min = lPromise(mkArg(argc, argv, 1), "Enter minimum:");
    long max = lPromise(mkArg(argc, argv, 2), "Enter maximum:");

    for (int i = min; i <= max; i++) {
        std::cout << "Teiler von " << i << ": ";
        for (int j = 1; j <= i; j++) {
            if (i % j == 0) {
                std::cout << j << ", ";
            }
        }

        std::cout << "\n";
    }

    return 0;
}