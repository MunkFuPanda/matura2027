/* author: Markus Spitzer
 * date: 2023-10-05
 * file: Lab2.6.cpp
 * class: 2CHIF
 */

#include <iostream>

#include "validator.h"

int main(int argc, char **argv) {
    long h = lPromise(mkArg(argc, argv, 1), "Höhe: ");

    for (int i = 0; i < h; i++) {
        for (int j = 0; j <= i; j++) {
            std::cout << "*";
        }
        std::cout << "\n";
    }

    std::cout << "\n";

    for (int i = h; i > 0; i--) {
        for (int j = 0; j < i; j++) {
            std::cout << "*";
        }
        std::cout << "\n";
    }

    std::cout << "\n";

    for (int i = h; i > 0; i--) {
        for (int j = 0; j < h; j++) {
            std::cout << ((j + 1) < i ? " " : "*");
        }
        std::cout << "\n";
    }

    std::cout << "\n";

    for (int i = h; i > 0; i--) {
        for (int j = h; j > 0; j--) {
            std::cout << (j > i ? " " : "*");
        }
        std::cout << "\n";
    }

    std::cout << "\n";

    for (int i = 0; i < (h - 1); i++) {
        for (int j = 0; j <= i; j++) {
            std::cout << "*";
        }
        std::cout << "\n";
    }
    for (int i = h; i > 0; i--) {
        for (int j = 0; j < i; j++) {
            std::cout << "*";
        }
        std::cout << "\n";
    }

    std::cout << "\n";

    for (int i = h; i > 0; i--) {
        for (int j = 0; j < h; j++) {
            std::cout << ((j + 1) < i ? " " : "*");
        }
        std::cout << "\n";
    }
    for (int i = (h - 1); i > 0; i--) {
        for (int j = h; j > 0; j--) {
            std::cout << (j > i ? " " : "*");
        }
        std::cout << "\n";
    }

    return 0;
}