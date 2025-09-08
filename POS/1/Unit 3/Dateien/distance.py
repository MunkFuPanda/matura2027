"""
author: Markus Spitzer
file: distance.py
"""

import turtle
from math import *

t = turtle.numinput("distance.py", "Strecke in Meter:")
g = 9.81 # m/s²
s = g/2 * t**2

print("Die Dauer beträgt " + str(s) + "s")
