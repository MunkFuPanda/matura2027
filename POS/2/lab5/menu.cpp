/* author: Markus Spitzer
 * date: 2024-01-01
 * file: menu.cpp
 * class: 2CHIF
 */

#include "menu.h"

#include <cstring>
#include <iomanip>
#include <iostream>
#include <string>

int getselection() {
    int sel;

    do {
        std::string input;
        std::cout << "                M E N U\n"
                  << "                *******\n"
                  << "               iterative   recursive\n"
                  << "insert             1           2\n"
                  << "print              3           4\n"
                  << "print reverse                  6\n"
                  << "find               7           8\n"
                  << "remove             9          10\n"
                  << "get_size          11          12\n"
                  << "destroy           13          14\n"
                  << "end               15\n"
                  << "------------------------------------------\n"
                  << "your choice: ";

        std::cin >> input;
        sel = getInt((char*)input.c_str());
    } while (sel == ERR_SELECTION);
    return sel;
}

int getInt(char* value) {
    char* error;
    int num = strtol(value, &error, 10);
    if (strlen(error)) return ERR_SELECTION;
    return num;
}