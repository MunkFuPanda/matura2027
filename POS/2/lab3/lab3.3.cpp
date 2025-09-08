/* author: Markus Spitzer
 * date: 2023-10-20
 * file: Lab3.3.cpp
 * class: 2CHIF
 */

#include "Lab3.3.h"

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
        if (i % 2 == 0) {
            sum += i;
        }
    }

    return sum;
}

int sum_rec(int min, int max) {
    if (max > min) {
        if (max % 2 == 0) {
            return max += sum_rec(min, max - 1);
        }
        return (max - 1) + sum_rec(min, max - 2);
    }
    return 0;
}