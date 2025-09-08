/* author: Markus Spitzer
 * date: 2023-10-04
 * file: validator.h
 * class: 2CHIF
 */

#include <cstring>
#include <iostream>
#include <string>

const char *FILL_CHAR = "\\";

bool validateDouble(const char *in, double *out) {
    char *error;
    *out = std::strtod(in, &error);
    return !std::strlen(error);
}

bool validateLong(const char *in, long *out) {
    char *error;
    *out = std::strtol(in, &error, 10);
    return !std::strlen(error);
}

bool validateChar(const char *in, char *out) {
    if (std::strlen(in) != 1 || in[0] == *FILL_CHAR) {
        return false;
    }

    *out = in[0];
    return true;
}

long lPromise(const char *argv, const char *prompt) {
    long l;

    std::string value = argv;
    while (!validateLong(value.c_str(), &l)) {
        std::cout << prompt;
        value.clear();
        std::cin >> value;
    }

    return l;
}

double dPromise(const char *argv, const char *prompt) {
    double d;

    std::string value = argv;
    while (!validateDouble(value.c_str(), &d)) {
        std::cout << prompt;
        std::cin >> value;
    }

    return d;
}

char cPromise(const char *argv, const char *prompt) {
    char c;

    std::string value = (std::string)argv;
    while (!validateChar(&value[0], &c)) {
        std::cout << prompt;
        std::cin >> value;
    }

    return c;
}

const char *mkArg(int argc, char **argv, int i) {
    const char *replacement = FILL_CHAR;
    return i < argc ? argv[i] : replacement;
}