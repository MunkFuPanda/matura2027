"""
author: Markus Spitzer
file: quadrate3.py
"""

from turtle import *


def draw(fill_color, pen_color):
    fillcolor(fill_color)
    pencolor(pen_color)
    begin_fill()
    
    forward(100)
    left(90)
    forward(100)
    left(90)
    forward(100)
    left(90)
    forward(100)
    left(90)
    
    end_fill()

    right(60)

pensize(5)
left(45)

draw("cyan", "red")
draw("magenta", "lime")
draw("yellow", "blue")
draw("red", "cyan")
draw("lime", "magenta")
draw("blue", "yellow")
