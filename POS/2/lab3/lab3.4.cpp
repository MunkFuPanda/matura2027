/* author: Markus Spitzer
 * date: 2023-10-20
 * file: Lab3.4.cpp
 * class: 2CHIF
 */

#include "Lab3.4.h"

#include <iostream>

int main() {
    int num = 0;
    std::cout << "Number: ";
    std::cin >> num;

    std::cout << "Faktorielle von " << num << " = " << fac(num) << "\n";
    std::cout << "Faktorielle von " << num << " = " << fac_rec(num) << "\n";

    return 0;
}

int fac(int num) {
    int product = 1;
    for (int i = 1; i <= num; i++) {
        product *= i;
    }
    return product;
}

int fac_rec(int num) {
    if (num > 1) {
        return num * fac_rec(num - 1);
    }
    return 1;
}