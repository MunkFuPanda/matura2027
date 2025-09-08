/* author: Markus Spitzer
 * date: 2023-10-20
 * file: Lab3.2.cpp
 * class: 2CHIF
 */

#include "Lab3.2.h"

#include <iostream>

int main() {
    int min = 0;
    std::cout << "Minimum: ";
    std::cin >> min;

    int max = 0;
    std::cout << "Maxiumum: ";
    std::cin >> max;

    std::cout << "Zahlen von " << min << "-" << max << " = " << sum(min, max)
              << "\n";
    std::cout << "Zahlen von " << min << "-" << max << " = "
              << sum_rec(min, max) << "\n";

    return 0;
}

int sum(int min, int max) {
    int sum = 0;
    for (int i = min; i <= max; i++) {
        sum += i;
    }

    return sum;
}

int sum_rec(int min, int max) {
    if (max > min) {
        return max + sum_rec(min, max - 1);
    }
    return min;
}