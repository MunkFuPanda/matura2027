all: main.o queue.o node.o
	g++ main.o queue.o node.o -o all.o

node.o:	node.cpp node.h
	g++ -c node.cpp -o node.o

queue.o: queue.cpp queue.h
	g++ -c queue.cpp -o queue.o

main.o:	main.cpp
	g++ -c main.cpp -o main.o

clean:
	rm -rf *.o