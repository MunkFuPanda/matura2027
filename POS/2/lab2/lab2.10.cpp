/* author: Markus Spitzer
 * date: 2023-10-09
 * file: Lab2.10.cpp
 * class: 2CHIF
 */

#include <iostream>

#include "validator.h"

int main(int argc, char **argv) {
    int max = lPromise(mkArg(argc, argv, 1), "Maximum: ");

    for (int i = 2; i <= max; i++) {
        bool isPrime = true;

        for (int j = 2; j < i; j++) {
            if (i % j == 0) {
                isPrime = false;
                break;
            }
        }

        if (isPrime) {
            std::cout << i << "\n";
        }
    }
}