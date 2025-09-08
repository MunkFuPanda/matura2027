"""
author: Markus Spitzer
file: draw-linfunc.py
"""

from turtle import *

grid_size = 20
tick_size = 5

# x achse
for i in range(1, 11, 1):
    forward(grid_size)
    left(90)
    forward(tick_size)

    penup()
    back(20 + tick_size)
    write(i)
    forward(20)
    pendown()

    right(90)

forward(grid_size)


home()
left(90)

# y achse
for i in range(1, 11, 1):
    forward(grid_size)
    right(90)
    forward(tick_size)

    penup()
    back(20 + tick_size)
    write(i)
    forward(20)
    pendown()

    left(90)

forward(grid_size)

# actual drawing the stat
d = numinput("D", "Enter D:")
k = numinput("K", "Enter K:")

penup()
home()
forward(d  * grid_size)
left(90)
forward(d * k * 20)
setheading(towards(0, 0))
goto(0, 0)
right(180)
pendown()

forward(1000)
