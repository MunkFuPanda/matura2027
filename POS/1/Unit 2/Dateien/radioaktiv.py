"""
author: Markus Spitzer
file: radioaktiv.py
"""

from turtle import *
#quadrat-offset
back(400 / 2)
right(90)
back(400 / 2)
left(90)
#quadrat
pensize(20)
fillcolor("yellow")
begin_fill()

for i in range(0, 4, 1):
    forward(400)
    right(90)

penup()
home()
right(90)
end_fill()
fillcolor("black")
pendown()
#sektoren
for i in range(0, 3, 1):
    begin_fill()
    right(50/2)
    forward((400 / 8) * 3)
    left(90)
    circle((400 / 8) * 3, 50)
    left(90)
    forward((400 / 8) * 3)
    right(50 / 2 + 180)
    end_fill()
    right(360/3)
#kreis-offset
right(90)
forward(40)
left(90)
#kreis
pencolor("yellow")
begin_fill()
circle(40)
end_fill()
