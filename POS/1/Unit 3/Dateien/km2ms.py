"""
author: Markus Spitzer
file: kmh2ms.py
"""

import turtle
from math import *

km = turtle.numinput("km2ms.py", "Geben Sie eine Zahl km/h ein, die zu m/s konvertiert werden sollen.")


print(str(km) + " km/h = " + str(km / 3.6) + " m/s")
