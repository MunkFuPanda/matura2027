"""
author: Markus Spitzer
file: sumdigits.py
"""

a = input("Geben Sie eine Zahl an: ")
c = 0

for b in a:
    try:
        c = c + int(b)
    except:
        pass

print(c)

