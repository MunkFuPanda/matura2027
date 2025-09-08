#include "queue.h"

#include <iostream>

void put(node** r, node** l, int value) {
    // B, E bzw. H
    if (!*r)
        *r = *l = new node{value, nullptr};
    else {
        (*l)->next = new node{value, nullptr};
        *l = (*l)->next;
    }
    // C, F bzw. I
}

int get(node** r, node** l) {
    // K
    if (!*r) return EMPTY_STACK;

    node* h = *r;
    *r = h->next;

    if (*l == h) *l = nullptr;

    int value = h->value;
    delete h;

    return value;
    // L
}

void print_iterative(node* r) {
    std::cout << std::endl;
    std::cout << "----" << std::endl;
    while (r) {
        std::cout << r->value << std::endl;
        r = r->next;
    }
    std::cout << "----" << std::endl;
    std::cout << std::endl;
}

void _print_recursive(node* r) {
    if (!r) return;
    std::cout << r->value << std::endl;
    _print_recursive(r->next);
}

void print_recursive(node* r) {
    std::cout << std::endl;
    std::cout << "++++" << std::endl;
    _print_recursive(r);
    std::cout << "++++" << std::endl;
    std::cout << std::endl;
}

void _print_reverse_recursive(node* r) {
    if (!r) return;
    _print_reverse_recursive(r->next);
    std::cout << r->value << std::endl;
}

void print_reverse_recursive(node* r) {
    std::cout << std::endl;
    std::cout << "****" << std::endl;
    _print_reverse_recursive(r);
    std::cout << "****" << std::endl;
    std::cout << std::endl;
}

int size_iterative(node* r) {
    int count = 0;
    while (r) {
        count++;
        r = r->next;
    }
    return count;
}

int size_recursive(node* r) {
    if (!r) return 0;
    return size_recursive(r->next) + 1;
}

void destroy_iterative(node** r, node** l) {
    while (get(r, l) != EMPTY_STACK)
        ;
}

void destroy_recursive(node** r, node** l) {
    if (!*r) return;
    destroy_recursive(&(*r)->next, l);
    node* h = *r;
    std::cout << "deleting " << h->value << std::endl;
    *r = h->next;
    delete h;
}
