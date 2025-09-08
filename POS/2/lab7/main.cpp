#include <iostream>
#include "dll.h"

using namespace std;

int main() {
	dll* list = new dll();

	
	list->insert_iterative_head(20);
	list->insert_iterative_head(40);
	list->insert_iterative_head(10);
	list->insert_iterative_head(80);
	list->insert_iterative_head(60);
	
	cout << "sum iterative form head: " << list->sum_iterative_head() << endl;
	cout << "sum iterative form tail: " << list->sum_iterative_tail() << endl;
	cout << "sum recursive form head: " << list->sum_recursive_head() << endl;
	cout << "sum recursive form tail: " << list->sum_recursive_tail() << endl;

	list->print_iterative_head_ascending();
	list->print_recursive_head_ascending();
	list->print_recursive_head_descending();
	list->print_iterative_tail_descending();
	list->print_recursive_tail_ascending();
	list->print_recursive_tail_descending();

	delete list;
	list = new dll();

	list->insert_iterative_tail(20);
	list->insert_iterative_tail(40);
	list->insert_iterative_tail(10);
	list->insert_iterative_tail(80);
	list->insert_iterative_tail(60);


	list->print_iterative_head_ascending();
	list->print_recursive_head_ascending();
	list->print_recursive_head_descending();
	list->print_iterative_tail_descending();
	list->print_recursive_tail_ascending();
	list->print_recursive_tail_descending();


	delete list;
	list = new dll();

	list->insert_recursive_head(20);
	list->insert_recursive_head(40);
	list->insert_recursive_head(10);
	list->insert_recursive_head(80);
	list->insert_recursive_head(60);
	

	list->print_iterative_head_ascending();
	list->print_recursive_head_ascending();
	list->print_recursive_head_descending();
	list->print_iterative_tail_descending();
	list->print_recursive_tail_ascending();
	list->print_recursive_tail_descending();


	delete list;
	list = new dll();

	list->insert_recursive_tail(20);
	list->insert_recursive_tail(40);
	list->insert_recursive_tail(10);
	list->insert_recursive_tail(80);
	list->insert_recursive_tail(60);
	

	list->print_iterative_head_ascending();
	list->print_recursive_head_ascending();
	list->print_recursive_head_descending();
	list->print_iterative_tail_descending();
	list->print_recursive_tail_ascending();
	list->print_recursive_tail_descending();


	delete list;
	list = new dll();

	list->insert_iterative_head(20);
	list->insert_iterative_head(40);
	list->insert_iterative_head(10);
	list->insert_iterative_head(80);
	list->insert_iterative_head(60);

	
	list->remove_iterative_head(40);
	list->print_iterative_head_ascending();
	list->print_recursive_head_ascending();
	list->print_recursive_head_descending();
	list->print_iterative_tail_descending();
	list->print_recursive_tail_ascending();
	list->print_recursive_tail_descending();
	list->remove_iterative_head(80);
	list->print_iterative_head_ascending();
	list->print_recursive_head_ascending();
	list->print_recursive_head_descending();
	list->print_iterative_tail_descending();
	list->print_recursive_tail_ascending();
	list->print_recursive_tail_descending();
	list->remove_iterative_head(10);
	list->print_iterative_head_ascending();
	list->print_recursive_head_ascending();
	list->print_recursive_head_descending();
	list->print_iterative_tail_descending();
	list->print_recursive_tail_ascending();
	list->print_recursive_tail_descending();
	list->remove_iterative_head(20);
	list->print_iterative_head_ascending();
	list->print_recursive_head_ascending();
	list->print_recursive_head_descending();
	list->print_iterative_tail_descending();
	list->print_recursive_tail_ascending();
	list->print_recursive_tail_descending();
	list->remove_iterative_head(60);
	list->print_iterative_head_ascending();
	list->print_recursive_head_ascending();
	list->print_recursive_head_descending();
	list->print_iterative_tail_descending();
	list->print_recursive_tail_ascending();
	list->print_recursive_tail_descending();

	delete list;
	list = new dll();

	list->insert_iterative_head(20);
	list->insert_iterative_head(40);
	list->insert_iterative_head(10);
	list->insert_iterative_head(80);
	list->insert_iterative_head(60);

	
	list->remove_iterative_tail(40);
	list->remove_iterative_tail(80);
	list->remove_iterative_tail(10);
	list->remove_iterative_tail(20);
	list->remove_iterative_tail(60);
	

	delete list;
	list = new dll();

	list->insert_iterative_head(20);
	list->insert_iterative_head(40);
	list->insert_iterative_head(10);
	list->insert_iterative_head(80);
	list->insert_iterative_head(60);

	
	list->remove_recursive_head(40);
	list->remove_recursive_head(80);
	list->remove_recursive_head(10);
	list->remove_recursive_head(20);
	list->remove_recursive_head(60);


	delete list;
	list = new dll();

	list->insert_iterative_head(20);
	list->insert_iterative_head(40);
	list->insert_iterative_head(10);
	list->insert_iterative_head(80);
	list->insert_iterative_head(60);

	list->remove_recursive_tail(40);
	list->remove_recursive_tail(80);
	list->remove_recursive_tail(10);
	list->remove_recursive_tail(20);
	list->remove_recursive_tail(60);
	

	delete list;
	list = new dll();

	list->insert_iterative_head(20);
	list->insert_iterative_head(40);
	list->insert_iterative_head(10);
	list->insert_iterative_head(80);
	list->insert_iterative_head(60);

	list->print_iterative_head_ascending();
	list->print_recursive_head_ascending();
	list->print_recursive_head_descending();
	list->print_iterative_tail_descending();
	list->print_recursive_tail_ascending();
	list->print_recursive_tail_descending();

	delete list;

}