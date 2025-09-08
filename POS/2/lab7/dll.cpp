#include <iostream>
using namespace std;

#include "dll.h"

dll::dll() {
	head = nullptr;
	tail = nullptr;
}

dll::~dll() {
	node* h = head;
	while (h != nullptr) {
		head = head->getNext();
		delete h;
		h = head;
	}

	head = nullptr;
	tail = nullptr;
}

void dll::insert_iterative_head(int value) {
	if (head == nullptr) {
		head = new node(value, nullptr, nullptr);
		tail = head;
	}
	else if (value < head->getValue()) {
		head = new node(value, nullptr, head);
		head->next->prev = head;

	}
	else if (value > tail->getValue()) {
		tail = new node(value, tail, nullptr);
		tail->prev->next = tail;
	}
	else {
		node* h = head;
		while (h->getValue() < value) {
			h = h->getNext();
		}
		h->prev = new node(value, h->prev, h);
		h->prev->prev->next = h->prev;
	}
}

void dll::insert_iterative_tail(int value) {
	if (head == nullptr) {
		tail = new node(value, nullptr, nullptr);
		head = tail;
	}
	else if (tail->getValue() < value) {
		tail = new node(value, tail, nullptr);
		tail->prev->next = tail;

	}
	else if (value < head->getValue()) {
		head = new node(value, nullptr, head);
		head->next->prev = head;
	}
	else {
		node* t = tail;
		while (value < t->getValue()) {
			t = t->getPrev();
		}

		t->next = new node(value, t, t->next);
		t->next->next->prev = t->next;
	}
}

void dll::insert_recursive_head(int value) {
	_insert_recursive_head(value, &head, &tail);
}

void dll::_insert_recursive_head(int value, node** h, node** t) {
	if ((*h) != nullptr && (value > (*h)->value)) {
		_insert_recursive_head(value, &((*h)->next), t);
	}
	else {
		if (*h != nullptr) {
			*h = new node(value, (*h)->prev, *h);
			(*h)->next->prev = *h;
		}
		else {
			*h = new node(value, tail, nullptr);
			tail = (*h);

		}
	}
}

void dll::insert_recursive_tail(int value) {
	_insert_recursive_tail(value, &head, &tail);
}

void dll::_insert_recursive_tail(int value, node** h, node** t) {
	if ((*t) != nullptr && (value < (*t)->value)) {
		_insert_recursive_head(value, h, &((*t)->prev));
	}
	else {
		if (*t != nullptr) {
			*t = new node(value, *t, (*t)->next);
			(*t)->prev->next = *t;
		}
		else {
			*t = new node(value, nullptr, head);
			head = (*t);

		}
	}
}

void dll::remove_iterative_head(int value) {
	node* h = head;
	while (h != nullptr && h->value != value)
		h = h->next;

	if (h == nullptr)
		return;
	else {
		if (head == tail) {
			head = nullptr;
			tail = nullptr;
		}
		else if (h == head) {
			head = head->next;
			head->prev = nullptr;
		}
		else if (h == tail) {
			tail = tail->prev;
			tail->next = nullptr;
		}
		else {
			h->prev->next = h->next;
			h->next->prev = h->prev;
		}
		delete h;
	}
}

void dll::remove_iterative_tail(int value) {
	node* t = tail;
	while (t != nullptr && t->value != value)
		t = t->prev;
	if (t == nullptr)
		return;
	else {
		if (head == tail) {
			head = nullptr;
			tail = nullptr;
		}
		else if (t == tail) {
			tail = tail->prev;
			tail->next = nullptr;
		}
		else if (t == head) {
			head = head->next;
			head->prev = nullptr;
		}
		else {
			t->prev->next = t->next;
			t->next->prev = t->prev;
		}
		delete t;
	}
}

void dll::remove_recursive_head(int value) {
	_remove_recursive_head(value, &head, &tail);
}


void dll::_remove_recursive_head(int value, node** h, node** t) {
	if (*h != nullptr && (*h)->value != value) {
		_remove_recursive_head(value, &((*h)->next), t);
	}
	else {
		if (*h == nullptr) {
			return;
		}
		else {
			if (head == tail) {
				head = nullptr;
				tail = nullptr;
				delete* h;
				
			}
			else if (*h == head) {
				node* h1 = head;
				head = head->next;
				head->prev = nullptr;
				delete h1;
				
			}
			else if (*h == tail) {
				tail = tail->prev;
				tail->next = nullptr;
				delete* h;
				
			}
			else {
				node* h1 = *h;
				(*h)->next->prev = (*h)->prev;		
				(*h) = (*h)->next;
				delete h1;
			}
			
		}
	}
}

void dll::remove_recursive_tail(int value) {
	_remove_recursive_tail(value, &head, &tail);
}

void dll::_remove_recursive_tail(int value, node** h, node** t) {
	if (*t != nullptr && (*t)->value != value) {
		_remove_recursive_tail(value, h, &((*t)->prev));
	}
	else {
		if (*t == nullptr)
			return;
		else {
			if (head == tail) {
				head = nullptr;
				tail = nullptr;
				delete *t;
			}
			else if (*t == head) {
				head = head->next;
				head->prev = nullptr;
				delete *t;
			}
			else if (*t == tail) {
				node* t1 = tail;
				tail = tail->prev;
				tail->next = nullptr;
				delete t1;
			}
			else {
				node* t1 = *t;
				(*t)->prev->next = (*t)->next;
				(*t) = (*t)->prev;
				delete t1;
			}
		}

	}
}


void dll::print_iterative_head_ascending() {
	node* h = head;
	cout << "---- print iterative from head ascending order ----" << endl;
	while (h != nullptr) {
		cout << h->getValue() << endl;
		h = h->getNext();
	}
	cout << "-----------------------------------" << endl;
}

void dll::print_recursive_head_ascending() {
	cout << "---- print recursive from head ascending order ----" << endl;
	_print_recursive_head_ascending(head);
	cout << "-----------------------------------" << endl;
}

void dll::_print_recursive_head_ascending(node* h) {
	if (h != nullptr) {
		cout << h->getValue() << endl;
		_print_recursive_head_ascending(h->getNext());
	}
}

void dll::print_recursive_head_descending() {
	cout << "---- print recursive from head descending order ----" << endl;
	_print_recursive_head_descending(head);
	cout << "-----------------------------------" << endl;
}

void dll::_print_recursive_head_descending(node* h) {
	if (h != nullptr) {
		_print_recursive_head_descending(h->getNext());
		cout << h->getValue() << endl;
	}
}

void dll::print_iterative_tail_descending() {
	node* h = tail;
	cout << "---- print iterative from head ascending order ----" << endl;
	while (h != nullptr) {
		cout << h->getValue() << endl;
		h = h->getPrev();
	}
	cout << "-----------------------------------" << endl;
}

void dll::print_recursive_tail_ascending() {
	cout << "---- print recursive from tail ascending order ----" << endl;
	_print_recursive_tail_ascending(tail);
	cout << "-----------------------------------" << endl;
}

void dll::_print_recursive_tail_ascending(node* h) {
	if (h != nullptr) {
		_print_recursive_tail_ascending(h->getPrev());
		cout << h->getValue() << endl;
	}
}

void dll::print_recursive_tail_descending() {
	cout << "---- print recursive from tail descending order ----" << endl;
	_print_recursive_tail_descending(tail);
	cout << "-----------------------------------" << endl;
}

void dll::_print_recursive_tail_descending(node* h) {
	if (h != nullptr) {
		cout << h->getValue() << endl;
		_print_recursive_tail_descending(h->getPrev());
	}
}

int dll::sum_iterative_head() {
	int sum = 0;
	node* h = head;
	while (h != nullptr) {
		sum += h->value;
		h = h->next;
	}
	return sum;
}

int dll::sum_iterative_tail() {
	int sum = 0;
	node* t = tail;
	while (t != nullptr) {
		sum += t->value;
		t = t->prev;
	}
	return sum;
}

int dll::sum_recursive_head() {
	return _sum_recursive_head(head);
}

int dll::sum_recursive_tail() {
	return _sum_recursive_tail(tail);
}

int dll::_sum_recursive_head(node* h) {
	if (h != nullptr)
		return h->value + _sum_recursive_head(h->next);
	return 0;
}

int dll::_sum_recursive_tail(node* t) {
	if (t != nullptr)
		return t->value + _sum_recursive_tail(t->prev);
	return 0;
}

