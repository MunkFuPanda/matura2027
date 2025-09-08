/* author: Markus Spitzer
 * date: 2023-10-02
 * file: calc.cpp
 * class: 2CHIF
 */

#include <cstring>
#include <iostream>
#include <string>

const int WRONG_ARGUMENT_COUNT = -1;
const int INVALID_OPERATOR = -2;
const int CONVERSION_ERROR = -3;
const int DIVISION_BY_ZERO = -4;

const std::string USAGE_STRING = "usage: <int> <operator> <int>";

int main(int argc, char **argv) {
    long num1 = 0, num2 = 0;
    char *error;

    if (argc != 4) {
        std::cout << USAGE_STRING << std::endl;
        return WRONG_ARGUMENT_COUNT;
    }

    if (strlen(argv[2]) != 1) {
        std::cout << USAGE_STRING << std::endl;
        return INVALID_OPERATOR;
    }

    num1 = strtol(argv[1], &error, 10);
    if (strlen(error)) {
        std::cout << USAGE_STRING << std::endl;
        return CONVERSION_ERROR;
    }

    num2 = strtol(argv[3], &error, 10);
    if (strlen(error)) {
        std::cout << USAGE_STRING << std::endl;
        return CONVERSION_ERROR;
    }

    int result;
    switch (argv[2][0]) {
        case '+':
            result = num1 + num2;
            break;
        case '-':
            result = num1 - num2;
            break;
        case '*':
            result = num1 * num2;
            break;
        case '/':
            if (num2 == 0) {
                std::cout << "Divisor cannot be 0" << std::endl;
                std::cout << USAGE_STRING << std::endl;
                return DIVISION_BY_ZERO;
            }
            result = num1 / num2;
            break;
        default:
            std::cout << USAGE_STRING << std::endl;
            return INVALID_OPERATOR;
    }

    std::cout << num1 << " " << argv[2][0] << " " << num2 << " = " << result
              << std::endl;
}
