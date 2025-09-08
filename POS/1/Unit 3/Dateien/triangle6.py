"""
author: Markus Spitzer
file: triangle6.py
"""

import turtle
from math import *

s1 = turtle.numinput("Kathete1", "Geben Sie einen Wert für die 1 Kathete ein: ")
s2 = turtle.numinput("Kathete2", "Geben Sie einen Wert für die 2 Kathete ein: ")
s3 = sqrt(s1 ** 2 + s2 ** 2)

A = (s1 * s2) / 2
U = s1 + s2 + s3

print(
"""
Seite 3: """ + str(s3) + """
Umfang: """ + str(U) + """
Fläche: """ + str(A))
