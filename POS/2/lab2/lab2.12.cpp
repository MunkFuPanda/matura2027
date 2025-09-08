/* author: Markus Spitzer
 * date: 2023-10-09
 * file: Lab2.12.cpp
 * class: 2CHIF
 */

#include <stdlib.h>
#include <time.h>

#include <iostream>

#include "validator.h"

int main(int argc, char **argv) {
    int iSecret, iGuess;
    const char *_first_guess;
    srand(time(NULL));

    iSecret = rand() % 100;

    do {
        std::cout << "Errate die Zahl (0 bis 99)\n";
        iGuess = lPromise(_first_guess, "Dein Tipp: ");
        if (iSecret < iGuess)
            std::cout << "kleiner!\n";
        else if (iSecret > iGuess)
            std::cout << "größer!\n";
    } while (iSecret != iGuess);

    std::cout << "Richtig!\n";
    return 0;
}