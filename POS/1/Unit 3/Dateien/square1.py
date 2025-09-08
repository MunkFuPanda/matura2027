"""
author: Markus Spitzer
file: square1.py
"""

import turtle

s = turtle.numinput("Square1.py", "Seitenlänge:")

print("Umfang: " + str(s * 4))
print("Fläche: " + str(s * s))
