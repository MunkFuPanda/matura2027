/* author: Markus Spitzer
 * date: 2023-10-05
 * file: Lab2.7.cpp
 * class: 2CHIF
 */

#include <iostream>

#include "validator.h"

int main(int argc, char **argv) {
    long h = lPromise(mkArg(argc, argv, 1), "Höhe: ");

    for (int i = 0; i < h; i++) {
        for (int j = 0; j <= i; j++) {
            if (j == 0 || j == i || i == (h - 1))
                std::cout << "*";
            else
                std::cout << " ";
        }
        std::cout << "\n";
    }

    std::cout << "\n";

    for (int i = h; i >= 1; i--) {
        for (int j = i; j >= 1; j--) {
            if (j == 1 || j == i || i == h)
                std::cout << "*";
            else
                std::cout << " ";
        }
        std::cout << "\n";
    }

    std::cout << "\n";

    for (int i = h; i > 0; i--) {
        for (int j = 0; j < h; j++) {
            std::cout << ((j + 1 == i || j + 1 == h || i == 1) ? "*" : " ");
        }
        std::cout << "\n";
    }

    std::cout << "\n";

    for (int i = h; i > 0; i--) {
        for (int j = h; j > 0; j--) {
            std::cout << ((j == i || i == h | j == 1) ? "*" : " ");
        }
        std::cout << "\n";
    }

    std::cout << "\n";

    for (int i = 0; i < h - 1; i++) {
        for (int j = 0; j <= i; j++) {
            if (j == 0 || j == i || i == (h - 1))
                std::cout << "*";
            else
                std::cout << " ";
        }
        std::cout << "\n";
    }
    for (int i = h; i >= 1; i--) {
        for (int j = i; j >= 1; j--) {
            if (j == 1 || j == i)
                std::cout << "*";
            else
                std::cout << " ";
        }
        std::cout << "\n";
    }
    std::cout << "\n";

    for (int i = h; i > 1; i--) {
        for (int j = 0; j < h; j++) {
            std::cout << ((j + 1 == i || j + 1 == h || i == 1) ? "*" : " ");
        }
        std::cout << "\n";
    }
    for (int i = h; i > 0; i--) {
        for (int j = h; j > 0; j--) {
            std::cout << ((j == i || j == 1) ? "*" : " ");
        }
        std::cout << "\n";
    }

    return 0;
}
