#include "node.h"

class dll {
		node* head;
		node* tail;
		void _insert_recursive_head(int value, node** h, node** t);
		void _insert_recursive_tail(int value, node** h, node** t);
		void _remove_recursive_head(int value, node** h, node**t);
		void _remove_recursive_tail(int value, node**h, node** t);

		void _print_recursive_head_ascending(node* h);
		void _print_recursive_head_descending(node* h);
		void _print_recursive_tail_ascending(node* h);
		void _print_recursive_tail_descending(node* h);
		int _sum_recursive_head(node* h);
		int _sum_recursive_tail(node* t);

	public:
		dll();
		~dll();
		void insert_iterative_head(int value);
		void insert_iterative_tail(int value);

		void insert_recursive_head(int value);
		void insert_recursive_tail(int value);

		void remove_iterative_head(int value);
		void remove_iterative_tail(int value);

		void remove_recursive_head(int value);
		void remove_recursive_tail(int value);

		void print_iterative_head_ascending();

		void print_recursive_head_ascending();
		

		void print_recursive_head_descending();
		

		void print_iterative_tail_descending();

		void print_recursive_tail_ascending();
		

		void print_recursive_tail_descending();

		int sum_iterative_head();
		int sum_iterative_tail();

		int sum_recursive_head();
		int sum_recursive_tail();

		

};