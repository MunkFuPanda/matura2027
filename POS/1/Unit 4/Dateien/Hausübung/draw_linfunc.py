"""
author: Markus Spitzer
file: draw_linfunc.py
"""

import turtle

# funktion um 1 einheit zu zeichnen

turtle.speed('fastest')


def draw_interval(show_count, number=""):
    turtle.forward(30)
    turtle.left(90)
    turtle.forward(5)
    turtle.back(10)

    if show_count:
        turtle.penup()
        turtle.back(20)
        turtle.write(number)
        turtle.forward(20)
        turtle.pendown()

    turtle.forward(5)
    turtle.right(90)

# funktion um das koordinatensystem zu zeichnen


def axis(show_count):
    turtle.penup()
    turtle.back(30 * 11)

    turtle.pendown()

    draw_interval(show_count, -10)
    draw_interval(show_count, -9)
    draw_interval(show_count, -8)
    draw_interval(show_count, -7)
    draw_interval(show_count, -6)
    draw_interval(show_count, -5)
    draw_interval(show_count, -4)
    draw_interval(show_count, -3)
    draw_interval(show_count, -2)
    draw_interval(show_count, -1)
    draw_interval(show_count)
    draw_interval(show_count, 1)
    draw_interval(show_count, 2)
    draw_interval(show_count, 3)
    draw_interval(show_count, 4)
    draw_interval(show_count, 5)
    draw_interval(show_count, 6)
    draw_interval(show_count, 7)
    draw_interval(show_count, 8)
    draw_interval(show_count, 9)
    draw_interval(show_count, 10)

    turtle.forward(30)


# lineare funktion zeichnen

def draw_line(x):
    # d offset zeichnen
    turtle.penup()
    turtle.home()
    turtle.left(90)
    turtle.forward(d * 30)
    turtle.pendown()

    turtle.goto(x=x * 30, y=(x * k + d) * 30)

    #turtle.setheading(turtle.towards(y=(x * k + d)*30, x=(k * 30)))
    #turtle.forward(300)
    #turtle.back(600)
    turtle.home()


# eingabe abfragen
d = float(input("Zahl für d:"))
k = float(input("Zahl für k:"))

# koordinatensystem zeichnen
axis(True)
turtle.penup()
turtle.home()
turtle.left(90)
axis(False)

""" draw_line(-10)
draw_line(-9)
draw_line(-8)
draw_line(-7)
draw_line(-6)
draw_line(-5)
draw_line(-4)
draw_line(-3)
draw_line(-2)
draw_line(-1)
draw_line(0)
draw_line(1)
draw_line(2)
draw_line(3)
draw_line(4)
draw_line(5)
draw_line(6)
draw_line(7)
draw_line(8)
draw_line(9)
draw_line(10) """