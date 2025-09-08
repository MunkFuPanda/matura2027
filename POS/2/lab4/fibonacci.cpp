/* author: Markus Spitzer
 * date: 2023-10-25
 * file: fibonacci.cpp
 * class: 2CHIF
 */

#include "fibonacci.h"

#include <iostream>

int main(int argc, char** argv) {
    int num;
    std::cout << "Größe eingeben: ";
    std::cin >> num;

    int* array = new int[num];
    fill(array, num);
    print(array, num);

    delete array;
    return 0;
}

void fill(int a[], int size) {
    a[0] = 1;
    a[1] = 1;

    for (int i = 1; i < size; i++) {
        a[i + 1] = a[i] + a[i - 1];
    }
}

void print(int a[], int size) {
    for (int i = 0; i < size; i++) {
        std::cout << a[i] << " ";
    }
    std::cout << std::endl;
}