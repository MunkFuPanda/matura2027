"""
author: Markus Spitzer
file: bmi.py
"""


def bmi(weight, height):
    return weight / (height ** 2)

weight = float(input("Gewicht in kg:"))
height = float(input("Größe in m:"))
r = bmi(weight, height)
_class = "Normal"

if r > 26:
    _class = "Übergewichtig"
elif r < 18.5:
    _class = "Untergewichtig"

print(r, _class)
