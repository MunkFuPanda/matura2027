/*
 * File:	menu.h
 * Autor:	Markus Spitzer 2CHIF
 * History:	2024-01-01
 */

#include <iostream>

enum MEN {
    MEN_INSERT_ITER = 1,
    MEN_INSERT_REC,
    MEN_PRINT_ITER,
    MEN_PRINT_REC,
    MEN_PRINT_REVERSE_REC = 6,
    MEN_FIND_ITER,
    MEN_FIND_REC,
    MEN_REMOVE_ITER,
    MEN_REMOVE_REC,
    MEN_GETSIZE_ITER,
    MEN_GETSIZE_REC,
    MEN_DESTROY_ITER,
    MEN_DESTROY_REC,
    MEN_END
};

const int ERR_SELECTION = -1;

int getInt(char* value);
int getselection();