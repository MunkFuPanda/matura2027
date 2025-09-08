"""
author: Markus Spitzer
file: power.py
"""

x = int(input("x: "))
y = int(input("y: "))
erg = 0

if y >= 0:
    if x == 1:
        erg = 1
    if x == -1 and y % 2 == 0:
        erg = 1
    if x == -1and y % 2 == 1:
        erg = -1
    if y == 0:
        erg = 1
    if y == 1:
        erg = x
    if y > 1:
        erg = x ** y
    print("Ergebnis: ", erg)
else:
    print("Ungültige Eingabe")
