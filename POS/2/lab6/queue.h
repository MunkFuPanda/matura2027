/*
 * File:	queue.h
 * Autor:	Markus Spitzer 2CHIF
 * History:	2024-03-13
 */

/*--------------------------- includes ------------------------------*/
#include <string>

#include "node.h"

/*--------------------------- defines -------------------------------*/

/*--------------------------- typedefs ------------------------------*/
class Queue {
   private:
    Node* root = nullptr;
    Node* last = nullptr;
    int _size_recursive(Node*);
    void _print_recursive(Node*);
    void _print_recursive_reverse(Node*);

   public:
    inline static const std::string EMPTY_QUEUE = "";
    Queue();
    ~Queue();
    void put(std::string);
    std::string get();
    void print_iterative();
    void print_recursive();
    void print_recursive_reverse();
    int size_iterative();
    int size_recursive();
};
/*--------------------------- globals -------------------------------*/

/*-------------------------- prototypes -----------------------------*/