"""
author: Markus Spitzer
file: squareroot.py
"""

import turtle
from math import *

x0 = turtle.numinput("Squareroot", "Enter a number: ")
a = x0

x1 = (x0 + a/x0)/2
x2 = (x1 + a/x1)/2
x3 = (x2 + a/x2)/2

print("Geschätzte Quadratwurzel: ", x3)
print("Reale Quadratwurzel: ", sqrt(a))
print("Differenz: ", abs(x3 - sqrt(a)))
