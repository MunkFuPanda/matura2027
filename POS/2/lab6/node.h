/*
 * File:	node.h
 * Autor:	Markus Spitzer 2CHIF
 * History:	2024-03-13
 */

/*--------------------------- includes ------------------------------*/
#include <string>
/*--------------------------- defines -------------------------------*/

/*--------------------------- typedefs ------------------------------*/
class Node {
   private:
    std::string value;
    Node* next = nullptr;

   public:
    Node(std::string);
    ~Node();
    void setNext(Node*);
    Node* getNext();
    std::string getValue();
};

/*--------------------------- globals -------------------------------*/

/*-------------------------- prototypes -----------------------------*/