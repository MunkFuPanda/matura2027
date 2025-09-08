"""
author: Markus Spitzer
file: sechseck01.py
"""

from turtle import *
from math import *

side_length = 60
radius = (sqrt(3) / 2) * side_length

penup()
forward(radius)
left(90)
pendown()

fillcolor("green")
begin_fill()

forward(30)

for i in range(0, 5, 1):
    left(60)
    forward(60)

left(60)
forward(30)

end_fill()

penup()

home()

done()