/* author: Markus Spitzer
 * date: 2023-10-18
 * file: Lab3.1.cpp
 * class: 2CHIF
 */

#include "Lab3.1.h"

#include <iostream>

int main() {
    int limit = 0;
    std::cout << "Limit: ";
    std::cin >> limit;

    std::cout << "Zahlen von 1-" << limit << " = " << sum(limit) << "\n";
    std::cout << "Zahlen von 1-" << limit << " = " << sum_rec(limit) << "\n";

    return 0;
}

int sum(int ob) {
    int sum = 0;
    for (int i = 1; i <= ob; i++) {
        sum += i;
    }

    return sum;
}

int sum_rec(int ob) {
    if (ob > 0) return ob + sum_rec(ob - 1);
    return 0;
}