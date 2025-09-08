#include <iostream>

#include "queue.h"

int main() {
    node* root = nullptr;
    node* last = nullptr;

    // A
    std::cout << "inserting 10, 20, 40" << std::endl;
    put(&root, &last, 10);
    // D
    put(&root, &last, 20);
    // G
    put(&root, &last, 40);
    // J

    print_iterative(root);
    print_recursive(root);
    print_reverse_recursive(root);
    std::cout << "size_iterative: " << size_iterative(root) << std::endl;
    std::cout << "size_recursive: " << size_recursive(root) << std::endl;

    std::cout << std::endl;
    std::cout << "get " << get(&root, &last) << std::endl;
    // M
    std::cout << "get " << get(&root, &last) << std::endl;
    std::cout << "get " << get(&root, &last) << std::endl;
    std::cout << "get " << get(&root, &last) << std::endl;

    print_iterative(root);
    print_recursive(root);
    print_reverse_recursive(root);

    std::cout << "inserting 10, 20, 40" << std::endl;
    put(&root, &last, 10);
    put(&root, &last, 20);
    put(&root, &last, 40);

    print_iterative(root);
    print_recursive(root);
    print_reverse_recursive(root);

    std::cout << "destroy iterative" << std::endl;
    destroy_iterative(&root, &last);

    print_iterative(root);
    print_recursive(root);
    print_reverse_recursive(root);

    std::cout << "inserting 10, 20, 40" << std::endl;
    put(&root, &last, 10);
    put(&root, &last, 20);
    put(&root, &last, 40);

    print_iterative(root);
    print_recursive(root);
    print_reverse_recursive(root);

    std::cout << "destroy recursive" << std::endl;
    destroy_recursive(&root, &last);

    print_iterative(root);
    print_recursive(root);
    print_reverse_recursive(root);

    put(&root, &last, 10);
    put(&root, &last, 20);
    put(&root, &last, 40);

    print_iterative(root);
    print_recursive(root);
    print_reverse_recursive(root);

    destroy_recursive(&root, &last);
}