"""
author: Markus Spitzer
file: salary.py
"""

import turtle

d = turtle.numinput("Gehaltsberechnung", "Geben Sie das Anfangsgehalt an")
x = d * 0.03
k = 5

print("Das Gehalt beträgt: " + str(k * x + d))
