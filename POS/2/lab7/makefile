
all:	node.o dll.o main.o
	g++ node.o dll.o main.o -o dll

node.o:	node.h node.cpp
	g++ -c node.cpp -o node.o

dll.o:	node.o dll.h dll.cpp
	g++ -c dll.cpp -o dll.o

main.o:	main.cpp
	g++ -c main.cpp -o main.o

clean:
	rm node.o
	rm dll.o
	rm main.o
	rm dll
