/* author: Markus Spitzer
 * date: 2023-10-09
 * file: Lab2.13.cpp
 * class: 2CHIF
 */

#include <iostream>
#include <vector>

#include "validator.h"

int main(int argc, char **argv) {
    int start = 2;
    int end = lPromise(mkArg(argc, argv, 1), "Limit: ");

    std::vector<bool> marked(end + 1, false);

    for (int i = start; i <= end; i++) {
        marked[i] = false;
    }

    for (int i = start; i <= end; i++) {
        if (!marked[i]) {
            for (int j = i * 2; j <= end; j += i) {
                marked[j] = true;
            }
        }
    }

    for (int i = start; i <= end; i++) {
        if (!marked[i]) {
            std::cout << i << " ";
        }
    }

    std::cout << "\n";

    return 0;
}
