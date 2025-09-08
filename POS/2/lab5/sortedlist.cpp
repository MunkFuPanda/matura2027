/* author: Markus Spitzer
 * date: 2024-01-01
 * file: sortedlist.cpp
 * class: 2CHIF
 */

#include "sortedlist.h"

#include <iostream>

void insert_iterative(node** r, std::string value) {
    if ((*r == nullptr) || (value < (*r)->value)) {
        *r = new node{value, *r};
    } else {
        node* h = *r;
        while ((h->next != nullptr) && (value > h->next->value)) h = h->next;
        h->next = new node{value, h->next};
    }
}

void insert_recursive(node** r, std::string value) {
    if ((*r == nullptr) || ((*r)->value > value)) {
        *r = new node{value, *r};
    } else
        insert_recursive(&((*r)->next), value);
}

void print_iterative(node* r) {
    std::cout << std::endl;
    std::cout << "----" << std::endl;
    while (r != nullptr) {
        std::cout << r->value << std::endl;
        r = r->next;
    }
    std::cout << "----" << std::endl;
    std::cout << std::endl;
}

void print_recursive(node* r) {
    std::cout << std::endl;
    std::cout << "----" << std::endl;
    _print_recursive(r);
    std::cout << "----" << std::endl;
    std::cout << std::endl;
}

void _print_recursive(node* r) {
    if (r != nullptr) {
        std::cout << r->value << std::endl;
        _print_recursive(r->next);
    }
}

void print_recursive_reverse(node* r) {
    std::cout << std::endl;
    std::cout << "----" << std::endl;
    _print_recursive_reverse(r);
    std::cout << "----" << std::endl;
    std::cout << std::endl;
}

void _print_recursive_reverse(node* r) {
    if (r != nullptr) {
        _print_recursive_reverse(r->next);
        std::cout << r->value << std::endl;
    }
}

bool remove_iterative(node** r, std::string value) {
    while (*r && (*r)->value != value) r = &(*r)->next;
    if (!*r) return false;

    node* h = *r;
    *r = (*r)->next;
    delete h;
    return true;
}

bool remove_recursive(node** r, std::string value) {
    if ((*r)->value == value) {
        node* h = *r;
        *r = h->next;
        delete h;
        return true;
    }

    if (!(*r)->next) return false;
    return remove_recursive(&(*r)->next, value);
}

void delete_iterative(node** r) {
    while (*r) {
        node* h = *r;
        *r = h->next;
        delete h;
    }
}

void delete_recursive(node** r) {
    if (!*r) return;
    delete_recursive(&(*r)->next);
    delete *r;
    *r = (*r)->next;
}

bool find_iterative(node* r, std::string value) {
    while (r) {
        if (r->value == value) return true;
        r = r->next;
    }

    return false;
}

bool find_recursive(node* r, std::string value) {
    if (r->value == value) return true;
    return find_recursive(r->next, value);
    return false;
}

int getsize_iterative(node* r) {
    int count = 1;
    for (; r && (r = r->next); count++)
        ;
    return count;
}

int getsize_recursive(node* r) {
    if (!r) return 0;
    return 1 + getsize_recursive(r->next);
}