const int EMPTY_STACK = -1;

struct node {
    int value;
    node* next;
};

void put(node** r, node** l, int value);
int get(node** r, node** l);
void print_iterative(node* r);
void print_recursive(node* r);
void print_reverse_recursive(node* r);
int size_iterative(node* r);
int size_recursive(node* r);
void destroy_iterative(node** r, node** l);
void destroy_recursive(node** r, node** l);
