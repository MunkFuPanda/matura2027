/* author: Markus Spitzer
 * date: 2024-03-13
 * file: main.cpp
 * class: 2CHIF
 */

#include <iostream>

#include "queue.h"

int main(int argc, char** argv) {
    Queue q1;
    q1.put("Hello");
    q1.put("World");
    q1.print_iterative();
    std::cout << q1.get() << std::endl;
    std::cout << q1.get() << std::endl;
    std::cout << q1.get() << std::endl;

    Queue* q2 = new Queue();
    q2->put("How");
    q2->put("Are");
    q2->put("You");
    q2->print_recursive();
    delete q2;

    return 0;
}