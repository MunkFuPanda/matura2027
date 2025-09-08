"""
author: Markus Spitzer
file: mathe.py
"""

from mathelib import *

m = int(input("""Auswählen:
1. arithmetisches Mittel
2. geometrisches Mittel
3. Addieren
4. Beschleunigungsrechnung
5. Strecke bei Beschleunigung
6. Modulo ersatz
7. Square Root ersatz
8. Division
>"""))

if m == 1:
    a = float(input("number 1:"))
    b = float(input("number 2:"))
    c = float(input("number 3:"))

    print(arith_mean(a, b, c))
elif m == 2:
    a = float(input("number 1:"))
    b = float(input("number 2:"))

    if (a * b) < 0:
        print("Das geometrische Mittel der Zahlen {} und {} kann nicht berechnet werden.".format(a, b))
    else:
        print(geom_mean(a, b))
elif m == 3:
    isNumber = int(input("enter 0 to add strings, enter 1 to add numbers"))

    a = input("number 1:")
    b = input("number 2:")

    if isNumber == 1:
        a = float(a)
        b = float(b)

    print("{} + {} = {}".format(a, b, sum(a, b)))
elif m == 4:
    t = float(input("Zeit:"))

    if t <= 0:
        print("Keine negative Zeit möglich")
    else:
        print(v(t))
elif m == 5:
    t = float(input("Zeit:"))

    if t <= 0:
        print("Keine negative Zeit möglich")
    else:
        print(s(t))
elif m == 6:
    a = float(input("number 1:"))
    b = float(input("number 2:"))

    print(mod(a, b))
elif m == 7:
    print(square_root(int(input("Enter a number:"))))
elif m == 8:
    a = float(input("number 1:"))
    b = float(input("number 2:"))

    print(div(a, b))