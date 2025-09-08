"""
author: Markus Spitzer
file: wabe.py
"""

from turtle import *

def draw(color):
    fillcolor(color)
    begin_fill()

    for i in range(0, 5, 1):
        forward(100)
        left(60)

    end_fill()


hideturtle()
left(30)

for i in range(0, 6, 1):
    draw("orange")
    left(120)

left(120)
draw("yellow")
forward(100)

done()