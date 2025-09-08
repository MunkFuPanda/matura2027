"""
author: Markus Spitzer
file: lists.py
"""


def concat(a, b):
    for i in b:
        a.append(i)
        
    return a


def insert(a, b, i):
    bx = b.copy()
    bx.reverse()
    
    for x in bx:
        a.insert(i, x)
    return a


def sort(a):
    a.sort()
    return a


def sort_descending(a):
    return sorted(a, key=lambda x: x, reverse=True)


def minimum(seq):
    try:
        return min(seq)
    except:
        return None


def maximum(seq):
    erg = sorted(seq, key=lambda x: x[1], reverse=True)
    return erg[0] if len(erg) > 0 else None


def find(seq, num):
    for counter, i in enumerate(seq):
        if i == num:
            return counter
    return -1


def getNumbers():
    numbers = []

    try:
        while True:
            numbers.append(int(input("Zahl: ")))
    except EOFError:
        if len(numbers) == 0:
            print("Es wurden keine Zahlen eingegeben!")
            return getNumbers()

    return numbers


print(minimum(getNumbers()))
