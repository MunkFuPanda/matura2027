/* author: Markus Spitzer
 * date: 2023-10-05
 * file: Lab2.1.cpp
 * class: 2CHIF
 */

#include <iostream>

#include "validator.h"

int main(int argc, char** argv) {
    long min = lPromise(mkArg(argc, argv, 1), "Minimaler Wert: ");
    long max = lPromise(mkArg(argc, argv, 2), "Maximaler Wert: ");

    for (int i = min; i <= max; i++) {
        if (i % 7 == 0) {
            std::cout << i << " ";
        }
    }

    std::cout << "\n";
}