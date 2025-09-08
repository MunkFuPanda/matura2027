/* author: Markus Spitzer
 * date: 2023-10-09
 * file: Lab2.11.cpp
 * class: 2CHIF
 */

#include <iostream>

#include "validator.h"

int main(int argc, char **argv) {
    double c1 = lPromise(mkArg(argc, argv, 1), "Zahl 1: ");
    char op;

    do {  // solange abfragen, bis op ein gültiger Operator ist.
        op = cPromise(mkArg(argc, argv, 2), "Operator: ");
    } while (op != '+' && op != '-' && op != '*' && op != '/');
    double c2 = lPromise(mkArg(argc, argv, 3), "Zahl 2: ");

    int result;
    switch (op) {
        case '+':
            result = c1 + c2;
            break;
        case '-':
            result = c1 - c2;
            break;
        case '*':
            result = c1 * c2;
            break;
        case '/':
            if (c2 == 0) {
                std::cout << "Division-By-Zero not possible!\n";
                return -1;
            }
            result = c1 / c2;
            break;
    }

    std::cout << c1 << " " << op << " " << c2 << " = " << result << std::endl;
}