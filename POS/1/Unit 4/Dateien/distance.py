"""
author: Markus Spitzer
file: distance.py
"""

from math import *

a = 50
b = 70

print("Mit math.abs ist das Ergebnis " + str(abs(a - b)))

ergebnis = a - b

if ergebnis < 0:
    ergebnis = b - a

print("Ohne math.abs ist das Ergebnis " + str(ergebnis))
