"""
author: Markus Spitzer
file: lists2.py
"""


def length(a):
    count = 0

    for i in a:
        try:
            int(i)
            count += 1
        except:
            pass

    return count


def count_numbers(a):
    count = 0

    for i in a:
        if type(i) == int:
            count += 1
        else:
            count += length(i)

    return count


def max_numbers(a):
    count = 0

    for i in a:
        if type(i) == int:
            if i > count:
                count = i
        else:
            if max_numbers(i) > count:
                count = max_numbers(i)
                
    return count


def add(a, b):
    c = []

    for count, i in enumerate(a):
        c.append(i + b[count])

    return c


def mul(a, b):
    c = []

    for count, i in enumerate(a):
        if len(b) > count:
            c.append(i * b[count])

    return c


def sub(a, b):
    for k, v in enumerate(a):
        if b[k]:
            a[k] -= b[k]

    return a

    
def filter_greater(a, number):
    b = []

    for i in a:
        if i > number:
            b.append(i)

    return b
