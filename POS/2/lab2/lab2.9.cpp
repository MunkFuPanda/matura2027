/* author: Markus Spitzer
 * date: 2023-10-09
 * file: Lab2.9.cpp
 * class: 2CHIF
 */

#include <iomanip>
#include <iostream>
#include <string>

#include "validator.h"

const int FIELD_WIDTH = 4;

void printTableIndex(std::string value) {
    std::cout << std::setw(FIELD_WIDTH / 2 + 1) << value
              << std::setw(FIELD_WIDTH / 2 + 1) << "|";
}

int main(int argc, char **argv) {
    long limit = lPromise(mkArg(argc, argv, 1), "Grenze: ");
    char op;

    do {  // solange abfragen, bis op ein gültiger Operator ist.
        op = cPromise(mkArg(argc, argv, 2), "Operator: ");
    } while (op != '+' && op != '*' && op != '%');

    for (int h = 0; h <= limit; h++) {
        for (int w = 0; w <= limit; w++) {
            if (w == 0) {
                if (h == 0) {
                    std::string str;
                    str.assign(1, op);
                    printTableIndex(str);
                } else {
                    printTableIndex(std::to_string(h));
                }

            } else if (h == 0 && w > 0) {
                printTableIndex(std::to_string(w));
            } else {
                switch (op) {
                    case '+':
                        printTableIndex(std::to_string(h + w));
                        break;
                    case '*':
                        printTableIndex(std::to_string(h * w));
                        break;
                    case '%':
                        printTableIndex(std::to_string(h % w));
                        break;
                }
            }
        }
        std::cout << "\n";
    }
}