"""
author: Markus Spitzer
file: mymath.py
"""


def power(y, x):
    if x == 1:
        return 1
    if x % 2 == 0 and y % 2 == 0:
        return 1
    if y == 0:
        return 1
    if y == 1:
        return x
    if y > 1:
        return x ** y
    if y < 0 or x == y == 0:
        return "Not valid."


def sqrt(x0):
    a = x0
    
    x1 = (x0 + a/x0)/2
    x2 = (x1 + a/x1)/2
    x3 = (x2 + a/x2)/2

    return x3


def minimum(x0, x1, x2):
    if x0 > x1 and x0 > x2:
        return x0
    elif x1 > x0 and x1 > x2:
        return x1
    else:
        return x2


def grading(score):
    if score > 100 or score < 0:
        return "Invalid input!"

    if score >= 91:
        return "Sehr gut"
    elif score >= 79:
        return "Gut"
    elif score >= 63:
        return "Befriedigend"
    elif score >= 51:
        return "Genügend"
    else:
        return "Nicht genügend"
