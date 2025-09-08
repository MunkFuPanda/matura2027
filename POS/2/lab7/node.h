
class dll;

class node {
	int value;
	node* prev;
	node* next;

	public:
		friend dll;
		node(int value, node* prev, node* next);
		~node();
		int getValue();
		node* getPrev();
		node* getNext();
};
