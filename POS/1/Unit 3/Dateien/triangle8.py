"""
author: Markus Spitzer
file: triangle8.py
"""

import turtle
from math import *

s1 = turtle.numinput("Kathete 1", "Gib den Wert für eine Kathete ein:")
A = turtle.numinput("Fläche", "Gib den Wert für die Fläche ein:")

s2 = (A * 2) / s1 # kathete
s3 = sqrt(s1 ** 2 + s2 ** 2) # hypothenuse
U = s1 + s2 + s3 # umfang

print(
"""
Andere Kathete: """ + str(s2) + """
Hypothenuse: """ + str(s3) + """
Umfang: """ + str(U))
