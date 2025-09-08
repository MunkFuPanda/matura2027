"""
author: Markus Spitzer
file: division.py
"""

import turtle

a = turtle.numinput("A", "Input für A")
b = turtle.numinput("B", "Input für B")

if a - b > 0:
    print((a*b)/(a-b))
else:
    print("Nenner darf nicht null sein!")
