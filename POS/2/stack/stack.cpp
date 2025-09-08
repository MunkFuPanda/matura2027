/* author: Markus Spitzer
 * date: 2023-12-09
 * file: stack.cpp
 * class: 2CHIF
 */

#include "stack.h"

#include <iostream>

int main() {
    Node* root = nullptr;

    push(&root, 10);
    push(&root, 20);
    push(&root, 30);
    push(&root, 40);
    print(root);

    std::cout << std::endl;
    std::cout << pop(&root) << std::endl;
    std::cout << pop(&root) << std::endl;
    std::cout << std::endl;

    print(root);
    reset(&root);
    print(root);
}

// fügt ein neunes Elements an der obersten Stelle
// des Stacks ein
void push(Node** r, int value) {
    Node* newNode = new Node(value);
    if (!*r) {
        *r = newNode;
    } else {
        newNode->next = *r;
        *r = newNode;
    }
}

// entfernt das oberste Element und liefert dessen
// Wert zurück (Speicher muss freigegeben werden!!)
int pop(Node** r) {
    if (!*r) {
        std::cout << "list empty!!\n";
        return -1;
    } else {
        Node* temp = *r;
        *r = (*r)->next;
        int popped = temp->data;
        delete temp;
        return popped;
    }
}

// gibt den gesamten Stack aus
void print(Node* r) {
    while (r) {
        std::cout << r->data << " ";
        r = r->next;
    }

    std::cout << std::endl;
}

// entfernt sämtliche Elemente aus dem Stack
// der belegte Speicher muss freigeben werden!
void reset(Node** r) {
    while (*r) {
        Node* temp = *r;
        *r = (*r)->next;
        delete temp;
    }
}
