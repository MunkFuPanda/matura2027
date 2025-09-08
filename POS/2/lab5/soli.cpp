/* author: Markus Spitzer
 * date: 2024-01-01
 * file: soli.cpp
 * class: 2CHIF
 */

#include "soli.h"

#include <iostream>

#include "menu.h"
#include "sortedlist.h"

int main(int argc, char** argv) {
    node* root = nullptr;

    if (argc > 1) {
        for (int i = 1; i < argc; i++) {
            if (i % 2)
                insert_iterative(&root, argv[i]);
            else
                insert_recursive(&root, argv[i]);
        }
        print_iterative(root);
        print_recursive_reverse(root);

        delete_recursive(&root);

        return 0;
    }

    int choice = 0;
    bool found = false;
    std::string value;

    while ((choice = getselection()) != MEN_END) {
        switch (choice) {
            case MEN_INSERT_ITER:
                insert_iterative(&root, getValue());
                break;
            case MEN_INSERT_REC:
                insert_recursive(&root, getValue());
                break;
            case MEN_PRINT_ITER:
                print_iterative(root);
                break;
            case MEN_PRINT_REC:
                print_recursive(root);
                break;
            case MEN_PRINT_REVERSE_REC:
                print_recursive_reverse(root);
                break;
            case MEN_FIND_ITER:
                value = getValue();
                found = find_iterative(root, value);
                std::cout << value;
                if (found)
                    std::cout << " found" << std::endl;
                else
                    std::cout << " not found" << std::endl;
                break;
            case MEN_FIND_REC:
                value = getValue();
                found = find_recursive(root, value);
                std::cout << value;
                if (found)
                    std::cout << " found" << std::endl;
                else
                    std::cout << " not found" << std::endl;
                break;
            case MEN_REMOVE_ITER:
                value = getValue();
                found = remove_iterative(&root, value);
                std::cout << value;
                if (found)
                    std::cout << " removed";
                else
                    std::cout << " not removed (not in list)";
                break;
            case MEN_REMOVE_REC:
                value = getValue();
                found = remove_recursive(&root, value);
                std::cout << value;
                if (found)
                    std::cout << " removed";
                else
                    std::cout << " not removed (not in list)";
                break;
            case MEN_GETSIZE_ITER:
                std::cout << "size of list = " << getsize_iterative(root);
                break;
            case MEN_GETSIZE_REC:
                std::cout << "size of list = " << getsize_recursive(root);
                break;
            case MEN_DESTROY_ITER:
                delete_iterative(&root);
                break;
            case MEN_DESTROY_REC:
                delete_recursive(&root);
                break;
        }
        std::cout << std::endl << std::endl;
    }

    delete_iterative(&root);
    return 0;
}

std::string getValue() {
    std::string value = "";
    std::cout << "enter value: ";
    std::cin >> value;
    return value;
}