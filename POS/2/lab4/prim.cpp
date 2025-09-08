/* author: Markus Spitzer
 * date: 2023-10-25
 * file: prim.cpp
 * class: 2CHIF
 */

#include "prim.h"

#include <iostream>

int main(int argc, char** argv) {
    int num;
    std::cout << "Limit eingeben: ";
    std::cin >> num;

    // die eingegebene Zahl als index inkludieren
    num++;

    bool* array = new bool[num];
    fill(array, num);
    print(array, num);

    delete array;
    return 0;
}

void fill(bool a[], int size) {
    a[0] = false;
    a[1] = false;

    for (int i = 2; i < size; i++) {
        a[i] = true;
    }

    for (int i = size; i > 1; i--) {
        if (a[i]) {
            for (int j = i - 1; j > 1; j--) {
                if (i % j == 0) {
                    a[i] = false;
                }
            }
        }
    }
}

void print(bool a[], int size) {
    for (int i = 2; i < size; i++) {
        if (a[i]) {
            std::cout << i << " ";
        }
    }

    std::cout << std::endl;
}