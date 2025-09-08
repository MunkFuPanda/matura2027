"""
author: Markus Spitzer
file: rectangle3.py
"""

import turtle

s1 = turtle.numinput("Square1.py", "Seitenlänge 1:")
s2 = turtle.numinput("Square1.py", "Seitenlänge 2:")

print("Umfang: " + str(s1 * 2 + s2 * 2))
print("Fläche: " + str(s1 * s2))
