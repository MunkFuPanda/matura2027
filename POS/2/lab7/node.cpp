#include "node.h"

node::node(int value, node* prev, node* next) {
	this->value = value;
	this->prev = prev;
	this->next = next;
}

node::~node() {

}

int node::getValue() {
	return value;
}

node* node::getPrev() {
	return prev;
}

node* node::getNext() {
	return next;
}