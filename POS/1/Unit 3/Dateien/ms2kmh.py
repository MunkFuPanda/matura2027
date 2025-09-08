"""
author: Markus Spitzer
file: ms2kmh.py
"""

import turtle
from math import *

ms = turtle.numinput("ms2kmh.py", "Geben Sie eine Zahl m/s ein, die zu km/h konvertiert werden sollen.")


print(str(ms) + " m/s = " + str(ms * 3.6) + " km/h")
