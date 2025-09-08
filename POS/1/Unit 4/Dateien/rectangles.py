"""
author: Markus Spitzer
file: rectangles.py
"""

import turtle

s1 = turtle.numinput("Enter Side 1", "Please enter a number for side a")
s2 = turtle.numinput("Enter Side 2", "Please enter a number for side b")

object_type = "Rechteck"
object_size = "großes"

if s1 == s2:
    object_type = "Quadrat"

A = s1 * s2

if A < 100:
    object_size = "kleines"
elif A < 200:
    object_size = "mittleres"

print(object_size + " " + object_type)

