"""
author: Markus Spitzer
filename: olympic_rings.py
"""

from turtle import *

pensize(8)

def turn(x, y, radius, angle=360):
    penup()
    goto(x,y)
    circle(radius, angle)
    pendown()

def ring(x,y,color="black", amount=360, block_reset=False):
    pencolor(color)
    penup()
    if not block_reset:
        goto(x, y)

    pendown()
    circle(50, amount)

ring(-55, 100, "blue")
ring(55, 100, "black")
ring(165, 100, "red")
ring(0, 45, "orange")
ring(110, 45, "green")

setheading(0)
ring(165, 100, "red", -20)
setheading(0)
ring(55, 100, "black", -20)
setheading(0)
turn(55, 100, 50, 50)
ring(55, 100, "black", 50, True)
setheading(0)
turn(-55, 100, 50, 50)
ring(-55, 100, "blue", 50, True)
setheading(0)
ring(110, 45, "green", 50)