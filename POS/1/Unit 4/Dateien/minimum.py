"""
author: Markus Spitzer
file: minimum.py
"""

import turtle

n1 = turtle.numinput("Zahl eingeben", "Gib eine Zahl ein")
n2 = turtle.numinput("Zahl eingeben", "Gib eine Zahl ein")
n3 = turtle.numinput("Zahl eingeben", "Gib eine Zahl ein")

if n1 < n2 and n1 < n3:
    print(n1)
elif n2 < n1 and n2 < n3:
    print(n2)
else:
    print(n3)
