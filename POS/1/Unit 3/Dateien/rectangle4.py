"""
author: Markus Spitzer
file: rectangle4.py
"""

import turtle

s1 = turtle.numinput("Square1.py", "Seitenlänge 1:")
u = turtle.numinput("Square1.py", "Umfang:")
s2 = (u/2)-s1

print("Seitenlänge 2: " + str(s2))
print("Fläche: " + str(s1 * s2))
