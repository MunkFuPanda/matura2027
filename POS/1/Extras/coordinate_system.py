"""
author: Markus Spitzer
file: coordinate_system.py
"""

from turtle import *

def axis(einheits_laenge: int, richtung: int, spiegelung: int):
    for i in range(1, 11):
        forward(einheits_laenge * spiegelung)
        left(90 * richtung)
        forward(10 * spiegelung)
        penup()
        back(25 * spiegelung)
        write(i * richtung)
        forward(15 * spiegelung)
        pendown()
        right(90 * richtung)


speed(9 ** 9)

axis(30, 1, 1)
home()
left(90)
axis(30, 1, 1)
home()
right(90)
axis(30, -1, 1)
home()
left(180)
axis(30, -1, 1)

hideturtle()
