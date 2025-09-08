"""
author: Markus Spitzer
file: rectangles2.py
"""

input_a = float(input("Seite A:"))
input_b = float(input("Seite B:"))


def perimeter(side_a, side_b):
    return 2 * (side_a + side_b)


def area(side_a, side_b):
    return side_a * side_b


def class_(int_area):
    if input_a == input_b:
        if int_area < 100:
            return "kleines Quadrat"
        elif int_area >= 100:
            return "großes Quadrat"
    else:
        if int_area < 200:
            return "kleines Rechteck"
        elif int_area >= 200:
            return "großes Rechteck"


print("Umfang: {}cm\nFläche: {}cm²\nKlasse: {}".format(perimeter(input_a, input_b), area(input_a, input_b), class_(area(input_a, input_b))))