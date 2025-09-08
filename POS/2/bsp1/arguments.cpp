/* author: Markus Spitzer
 * date: 2023-10-02
 * file: arguments.cpp
 * class: 2CHIF
 */

#include <cstring>
#include <iostream>

int main(int argc, char **argv) {
    long int_count = 0;
    long int_sum = 0;
    long float_count = 0;
    double float_sum = 0;
    long invalid_count = 0;
    char *error;

    for (int i = 1; i < argc; i++) {
        int val = strtol(argv[i], &error, 10);
        if (!strlen(error)) {
            int_count++;
            int_sum += val;
            continue;
        }

        double val = strtod(argv[i], &error);
        if (!strlen(error)) {
            float_count++;
            float_sum += val;
            continue;
        }

        invalid_count++;
    }

    std::cout << "int count: " << int_count << std::endl
              << "int sum: " << int_sum << std::endl
              << "float count: " << float_count << std::endl
              << "float sum: " << float_sum << std::endl
              << "invalid count: " << invalid_count << std::endl;
}
