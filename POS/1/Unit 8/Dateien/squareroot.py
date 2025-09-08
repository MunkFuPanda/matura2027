"""
author: Markus Spitzer
file: tst.py
"""

from math import *

a = int(input("Geben Sie eine Zahl an: "))
n = int(input("Geben Sie den Iterationswert an: "))

x = a

for nmb in range(n):
    x = (x + a/x)/2

print(f'Annäherung: {x}\nWurzel: {sqrt(a)}\nDifferenz: {abs(x - sqrt(a))}')
