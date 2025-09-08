"""
author: Markus Spitzer
filename: olympic_rings.py
"""

from turtle import *

length = -10
num_distance = 15

#testing
speed(10^10)

def resetTurtle():
    penup()
    home()
    pendown()

# x axis
for i in range(0, 10, 1):
    left(90)
    forward(length)
    penup()
    back(num_distance)
    if i > 0:
        write(i)
    forward(num_distance)
    pendown()
    back(length)
    right(90)
    forward(50)

#reset
resetTurtle()
left(90)

# y axis
for i in range(0, 10, 1):
    right(90)
    forward(length)
    penup()
    back(num_distance)
    if i > 0:
        write(i)
    forward(num_distance)
    pendown()
    back(length)
    left(90)
    forward(50)