"""
author: Markus Spitzer
filename: triangle2.py
"""


from turtle import *
from math import *

factor = 65

a = 3 * factor
c = 5 * factor
b = sqrt(c ** 2 - a ** 2)

forward(b)
left(90)
forward(a)
home()