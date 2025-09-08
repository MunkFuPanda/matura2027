"""
author: Markus Spitzer
file: 6.py
"""

import turtle


def spiral(angle, max_size):
    for i in range(0, max_size):
        turtle.forward(i)
        turtle.left(angle)

spiral(30, 100)
