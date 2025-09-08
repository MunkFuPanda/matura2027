"""
author: Markus Spitzer
filename: sort.py
"""


a = int(input("a:"))
b = int(input("b:"))
c = int(input("c:"))

x1 = 0
x2 = 0
x3 = 0

if a < b and a < c:
    x3 = a
    if b < c:
        x2 = b
        x1 = c
    else:
        x2 = c
        x1 = b
elif b < a and b < c:
    x3 = b
    if a < c:
        x2 = a
        x1 = c
    else:
        x2 = c
        x1 = a
elif c < a and c < b:
    x3 = c
    if a < b:
        x2 = a
        x1 = b
    else:
        x2 = b
        x1 = a

print("1.", x3)
print("2.", x2)
print("3.", x1)
