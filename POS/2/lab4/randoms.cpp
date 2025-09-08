/* author: Markus Spitzer
 * date: 2023-10-25
 * file: Lab4.1.cpp
 * class: 2CHIF
 */

#include "randoms.h"

#include <iostream>

int main(int argc, char** argv) {
    int size, min, max;
    std::cout << "Größe des Arrays: ";
    std::cin >> size;
    std::cout << "Minimum: ";
    std::cin >> min;
    std::cout << "Maximum: ";
    std::cin >> max;

    int* array = new int[size];
    fill(array, size, min, max);
    print(array, size);
    std::cout << std::endl << sum(array, size) << std::endl << std::endl;

    sort(array, size);
    print(array, size);

    delete array;
    return 0;
}

void fill(int a[], int size, int min, int max) {
    srand(time(NULL));
    int random;
    bool isUnique;

    if ((max - min + 1) < size) {
        std::cout << "Größe zu klein!" << std::endl;
        return exit(-1);
    }

    for (int i = 0; i < size; i++) {
        do {
            isUnique = true;
            random = min + rand() % (max - min + 1);

            for (int j = 0; j < i; j++) {
                if (a[j] == random) {
                    isUnique = false;
                    break;
                }
            }
        } while (!isUnique);

        a[i] = random;
    }
}

void print(int a[], int size) {
    for (int i = 0; i < size; i++) {
        std::cout << a[i] << " ";
    }
    std::cout << std::endl;
}

int sum(int a[], int size) {
    int summe = 0;
    for (int i = 0; i < size; i++) {
        summe += a[i];
    }

    return summe;
}

void sort(int a[], int size) {
    int rounds = 0, checks = 0;
    bool flag;

    for (int i = 0; i < size; i++) {
        rounds++;
        flag = false;

        for (int j = 0; j < (size - i - 1); j++) {
            checks++;
            if (a[j] > a[j + 1]) {
                flag = true;
                int temp = a[j];
                a[j] = a[j + 1];
                a[j + 1] = temp;
            }
        }

        if (flag == false) {
            break;
        }
    }

    std::cout << checks << " Versuche in " << rounds << " Runden!\n";
}