"""
author: Markus Spitzer
file: haus01.py
"""

from turtle import *

hideturtle()

fillcolor("yellow")
begin_fill()

for i in range(0, 4, 1):
    forward(100)
    right(90)

end_fill()

fillcolor("red")
begin_fill()

left(60)
forward(100)
right(120)
forward(100)

end_fill()