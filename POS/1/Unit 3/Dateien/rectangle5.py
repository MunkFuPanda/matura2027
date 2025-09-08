"""
author: Markus Spitzer
file: rectangle5.py
"""

import turtle
from math import *

u = turtle.numinput("Square1.py", "Umfang:")
a = turtle.numinput("Square1.py", "Fläche:")

s1 = (u - sqrt(u ** 2 - 16 * a))/4
s2 = u/2 - s1

print("Seitenlänge 1: " + str(s1))
print("Seitenlänge 2: " + str(s2))