/* author: Markus Spitzer
 * date: 2023-10-05
 * file: Lab2.8.cpp
 * class: 2CHIF
 */

#include <iostream>

#include "validator.h"

int main(int argc, char **argv) {
    long field_h = lPromise(mkArg(argc, argv, 1), "Höhe eines Feldes: ");
    long field_w = lPromise(mkArg(argc, argv, 2), "Breite eines Feldes: ");
    long board_h = lPromise(mkArg(argc, argv, 3), "Höhe in Feldern: ");
    long board_w = lPromise(mkArg(argc, argv, 4), "Breite in Feldern: ");

    for (int h = 0; h < board_h * field_h; h++) {
        for (int w = 0; w < board_w * field_w; w++) {
            if (h % (field_h * 2) < field_h)
                std::cout << (w % (field_w * 2) < field_w ? "*" : " ");
            else
                std::cout << (w % (field_w * 2) < field_w ? " " : "*");
        }
        std::cout << "\n";
    }
}