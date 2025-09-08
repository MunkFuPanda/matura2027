/*
 * File:	stack.h
 * Autor:	Markus Spitzer 2CHIF
 * History:	2023-12-08
 */

/*--------------------------- includes ------------------------------*/

/*--------------------------- defines -------------------------------*/
struct Node {
    int data;
    Node* next;
    Node(int value) : data(value), next(nullptr) {}
};

/*--------------------------- typedefs ------------------------------*/

/*--------------------------- globals -------------------------------*/

/*-------------------------- prototypes -----------------------------*/
void push(Node**, int);
int pop(Node**);
void print(Node*);
void reset(Node**);