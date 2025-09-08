"""
author: Markus Spitzer
file: regular_polygon3.py
"""

from turtle import *


def regular_polygon1(length, colors):
    for color in colors:
        pencolor((color[0],color[
            1],color[2]))
        forward(length)
        left(360 / len(colors))


colormode(255)


length = int(input("Seitenlänge:"))
side_amount = int(input("Seitenanzahl:"))
colors = ()

for i in range(side_amount):
    current_color = ()
    for x in range(3):
        current_color += (int(input(f'Farbe {i + 1}, Wert:{x + 1}:')),)

    colors += (current_color,)

regular_polygon1(length, colors)

