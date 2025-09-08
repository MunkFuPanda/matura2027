"""
author: Markus Spitzer
file: haus02.py
"""

from turtle import *

speed(1000000)

base_size = 200

fillcolor("yellow")
begin_fill()

for i in range(0, 4, 1):
    forward(base_size)
    right(90)

end_fill()

fillcolor("red")
begin_fill()

left(60)
forward(base_size)
right(120)
forward(base_size)

end_fill()

home()

penup()
fillcolor("brown")
forward(base_size/6)
right(90)
forward(base_size)

pendown()
begin_fill()
back(base_size/3*2)
left(90)
forward(base_size/3)
right(90)
forward(base_size/3*2)
end_fill()
penup()

home()
forward(base_size/6*4)
left(90)
back(base_size/2)

pendown()
fillcolor("cyan")
begin_fill()

for i in range(0, 4, 1):
    forward(base_size/4)
    right(90)

end_fill()

forward(base_size/8)
right(90)
forward(base_size/4)
right(90)
forward(base_size/8)
right(90)
forward(base_size/8)
right(90)
forward(base_size/4)

hideturtle()

done()