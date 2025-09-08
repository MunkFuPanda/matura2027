"""
author: Markus Spitzer
file: yingyang.py
"""

from turtle import *

pensize(5)

fillcolor("black")
begin_fill()

right(90)
circle(50, 180)
circle(100, 180)

left(180)
circle(-50, 180)

end_fill()

penup()
left(90)
forward(40)
right(90)
pendown()

pencolor("white")
fillcolor("white")
begin_fill()
circle(10, 360)
end_fill()
