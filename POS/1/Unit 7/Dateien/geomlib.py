"""
author: Markus Spitzer
file: geomlib.py
"""

import math
import turtle

def distance1(a, b):
    return abs(a - b)


def distance2(ax, ay, bx, by):
    ergx = abs(ax - bx)
    ergy = abs(ay - by)
    
    return round(math.sqrt(ergx ** 2 + ergy ** 2), 2)


def distance3(ax, ay, az, bx, by, bz):
    ergx = abs(ax - bx)
    ergy = abs(ay - by)
    ergz = abs(az - bz)
    
    return round(math.sqrt(ergx ** 2 + ergy ** 2 + ergz ** 2), 2)


def regular_polygon(x, y, edge_amount, length):
    turtle.penup()
    turtle.goto(x, y)
    turtle.pendown()
    
    for i in range(edge_amount):
        turtle.forward(length)
        turtle.right(edge_amount)

    return edge_amount * length


def polygon(x1, y1, x2, y2, x3, y3, x4=None, y4=None, x5=None, y5=None):
    turtle.hideturtle()
    turtle.penup()
    turtle.goto(x1, y1)
    turtle.pendown()
    turtle.goto(x2, y2)
    turtle.goto(x3, y3)

    x_l, y_l = x3, y3
    umfang = distance2(x1, y1, x2, y2) + distance2(x2, y2, x3, y3)

    if x4 != None and y4 != None:
        turtle.goto(x4, y4)
        umfang += distance2(x3, y3, x4, y4)
        x_l = x4
        y_l = y4
    if x5 != None and y5 != None:
        turtle.goto(x5, y5)
        umfang += distance2(x_l, y_l, x5, y5)
        x_l = x5
        y_l = y5

    turtle.goto(x1, y1)
    return (umfang + distance2(x1, y1, x_l, y_l))

def stars_triangle(amount):
    string = ""

    for i in range(1, amount + 1):
        #for x in range(0, i):
        string = "*" * i
        string += "\n"
            

    return string
