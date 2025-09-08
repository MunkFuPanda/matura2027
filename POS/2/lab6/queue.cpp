/* author: Markus Spitzer
 * date: 2024-03-13
 * file: queue.cpp
 * class: 2CHIF
 */

#include "queue.h"

#include <iostream>

Queue::Queue() {
}

Queue::~Queue() {
    while (get() != EMPTY_QUEUE)
        ;
}

void Queue::put(std::string value) {
    if (!root)
        last = root = new Node(value);
    else {
        last->setNext(new Node(value));
        last = last->getNext();
    }
}

std::string Queue::get() {
    if (!root) return EMPTY_QUEUE;
    Node* h = root;

    root = root->getNext();
    std::string value = h->getValue();

    delete h;
    return value;
}

void Queue::print_iterative() {
    std::cout << "----" << std::endl;
    for (Node* h = root; h; h = h->getNext()) {
        std::cout << h->getValue() << std::endl;
    }
    std::cout << "----" << std::endl;
}

void Queue::_print_recursive(Node* current) {
    if (!current) return;
    std::cout << current->getValue() << std::endl;
    _print_recursive_reverse(current->getNext());
}

void Queue::print_recursive() {
    std::cout << "++++" << std::endl;
    _print_recursive(root);
    std::cout << "++++" << std::endl;
}

void Queue::_print_recursive_reverse(Node* current) {
    if (!current) return;
    _print_recursive_reverse(current->getNext());
    std::cout << current->getValue() << std::endl;
}

void Queue::print_recursive_reverse() {
    std::cout << "****" << std::endl;
    _print_recursive_reverse(root);
    std::cout << "****" << std::endl;
}

int Queue::size_iterative() {
    int count = 0;
    for (Node* h = root; h; h = h->getNext()) count++;
    return count;
}

int Queue::_size_recursive(Node* current) {
    return 1 + _size_recursive(current->getNext());
}

int Queue::size_recursive() {
    return _size_recursive(root);
}