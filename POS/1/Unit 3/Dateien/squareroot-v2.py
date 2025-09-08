"""
author: Markus Spitzer
file: squareroot-v2.py
"""

import turtle
from math import *

x = turtle.numinput("Squareroot", "Enter a number: ")
re = turtle.numinput("Decimals", "Enter an amount of decimals: ")

a = x

for i in range(0, int(re), 1):
    x = (x + a/x)/2

print("Geschätzte Quadratwurzel: ", x)
print("Reale Quadratwurzel:      ", sqrt(a))
print("Differenz: ", abs(x - sqrt(a)))
