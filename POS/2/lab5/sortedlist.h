/*
 * File:	sortedlist.h
 * Autor:	Markus Spitzer 2CHIF
 * History:	2024-01-01
 */

#include <iostream>

struct node {
    std::string value;
    node* next;
};

void insert_iterative(node** r, std::string value);
void insert_recursive(node** r, std::string value);
void print_iterative(node* r);
void print_recursive(node* r);
void _print_recursive(node* r);
void print_recursive_reverse(node* r);
void _print_recursive_reverse(node* r);
bool remove_iterative(node** r, std::string value);
bool remove_recursive(node** r, std::string value);
void delete_iterative(node** r);
void delete_recursive(node** r);
bool find_iterative(node* r, std::string value);
bool find_recursive(node* r, std::string value);
int getsize_iterative(node* r);
int getsize_recursive(node* r);