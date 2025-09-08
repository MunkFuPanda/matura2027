"""
author: Markus Spitzer
file: 6_quadrate.py
"""

from turtle import *

pensize(5)
hideturtle()

colors = [
    ["cyan", "red"],
    ["magenta", "lime"],
    ["yellow", "blue"],
    ["red", "cyan"],
    ["lime", "magenta"],
    ["blue", "yellow"],
]

def draw(fill_color, pen_color, amount):
    fillcolor(fill_color)
    pencolor(pen_color)
    begin_fill()
    for i in range(0, amount, 1):
        forward(100)
        left(90)

    end_fill()

left(45)

for k,v in list(colors):
    draw(k, v, 4)
    right(60)
