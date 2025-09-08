"""
author: Markus Spitzer
filename: probiermethode.py
"""

x1 = 21
x2 = 22
x3 = 25
x4 = 43
x5 = 68
x6 = 94

# funktion um die probiermethode auszuführen
def probiermethode(n):
    ergebnis = ""
    
    if n >= 128:
        ergebnis += "1"
        n -= 128
    else:
        ergebnis += "0"

    if n >= 64:
        ergebnis += "1"
        n -= 64

    if n >= 32:
        ergebnis += "1"
        n -= 32
    else:
        ergebnis += "0"

    if n >= 16:
        ergebnis += "1"
        n -= 16
    else:
        ergebnis += "0"

    if n >= 8:
        ergebnis += "1"
        n -= 8
    else:
        ergebnis += "0"

    if n >= 4:
        ergebnis += "1"
        n -= 4
    else:
        ergebnis += "0"

    if n >= 2:
        ergebnis += "1"
        n -= 2
    else:
        ergebnis += "0"

    if n >= 1:
        ergebnis += "1"
        n -= 1
    else:
        ergebnis += "0"

    return ergebnis


print(probiermethode(x1))
print(probiermethode(x2))
print(probiermethode(x3))
print(probiermethode(x4))
print(probiermethode(x5))
print(probiermethode(x6))