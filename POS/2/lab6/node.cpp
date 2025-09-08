/* author: Markus Spitzer
 * date: 2024-03-13
 * file: node.cpp
 * class: 2CHIF
 */

#include "node.h"

Node::Node(std::string _value) {
    value = _value;
}

Node::~Node() {
}

void Node::setNext(Node* _next) {
    next = _next;
}

Node* Node::getNext() {
    return next;
}

std::string Node::getValue() {
    return value;
}