"""
author: Markus Spitzer
file: calculate.py
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


prompt_type = input("1 = power\n2 = sqrt\nselect your method:")

if prompt_type == "1":
    x = int(input("Input für basis:"))
    y = int(input("Input für exponent:"))
    
    print(x, "^", y, "=", power(y, x))
elif prompt_type == "2":
    print(sqrt(float(input("Input für sqrt:"))))



