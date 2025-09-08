"""
author: Markus Spitzer
file: triangle6.py
"""

import turtle
from math import *

s1 = turtle.numinput("Kathete1", "Geben Sie einen Wert für die 1 Kathete ein: ")
s3 = turtle.numinput("Hypothenuse", "Geben Sie einen Wert für die Hypothenuse ein: ")
s2 = sqrt(s3 ** 2 - s1 ** 2)

A = (s1 * s2) / 2
U = s1 + s2 + s3

print(
"""
Seite 3: """ + str(s3) + """
Umfang: """ + str(U) + """
Fläche: """ + str(A))
