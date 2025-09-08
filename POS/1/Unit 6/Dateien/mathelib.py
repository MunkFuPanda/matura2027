"""
author: Markus Spitzer
file: mathelib.py
"""

import math


def arith_mean(a, b, c):
    return ((a + b + c) / 3)


def geom_mean(a, b):
    if a * b < 0:
        return -1

    return math.sqrt(a * b)


def sum(a, b):
    return a + b


def v(t):
    if t <= 0:
        return -1

    return 9.81 * t


def s(t):
    if t <= 0:
        return -1

    return 9.81/2 * (t ** 2)


def mod(a, b):
    return a - (int(a / b) * b)


def square_root(a):
    if a > 0:
        return (a ** 0.5)
    return -1

def div(a, b):
    if b == 0:
        return 0
    return a / b