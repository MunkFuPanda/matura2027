"""
author: Markus Spitzer
file: quadrate.py
"""

from turtle import *

pensize(5)
length = numinput("Länge", "Geben Sie eine Seitenlänge > 60 ein:",
                  minval=60, maxval=200)

if length > 60:
    for i in range(0, 3, 1):
        penup()
        home()
        forward(length/2)
        left(90)
        back(length/2)
        pendown()

        for i in range(0, 4, 1):
            forward(length)
            left(90)

        length -= 20
