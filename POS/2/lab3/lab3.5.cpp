/* author: Markus Spitzer
 * date: 2023-10-20
 * file: Lab3.5.cpp
 * class: 2CHIF
 */

#include "Lab3.5.h"

#include <iostream>

int main() {
    int base = 0;
    std::cout << "Base: ";
    std::cin >> base;

    int exp = 0;
    std::cout << "Exponent: ";
    std::cin >> exp;

    std::cout << base << "^" << exp << " = " << pow(base, exp) << "\n";
    std::cout << base << "^" << exp << " = " << pow_rec(base, exp) << "\n";

    return 0;
}

int pow(int base, int exp) {
    int result = 1;
    for (int i = 0; i < exp; i++) {
        result *= base;
    }

    return result;
}

int pow_rec(int base, int exp) {
    if (exp > 1) {
        return base * pow_rec(base, exp - 1);
    }
    return base;
}