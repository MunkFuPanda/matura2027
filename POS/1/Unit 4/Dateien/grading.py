"""
author: Markus Spitzer
file: grading.py
"""

import turtle

points = turtle.numinput("Zahl eingeben", "Gib eine Zahl ein", minval=0, maxval=100)

if points > 90:
    print(1)
elif points > 78:
    print(2)
elif points > 62:
    print(3)
elif points > 50:
    print(4)
else:
    print(5)
