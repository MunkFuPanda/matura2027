"""
author: Markus Spitzer
file: regulary_polygon2.py
"""

from turtle import *


def regular_polygon1(length, colors):
    for color in colors:
        pencolor(color)
        forward(length)
        left(360 / len(colors))


length = int(input("Seitenlänge:"))
side_amount = int(input("Seitenanzahl:"))
colors = ()

for i in range(side_amount):
    colors += (input(f'Farbe {i + 1}:'),)

regular_polygon1(length, colors)