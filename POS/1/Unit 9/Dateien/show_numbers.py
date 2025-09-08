"""
author: Markus Spitzer
file: show_numbers.py
"""


def tabelle(dez, binaer, hexa):
    return "{0:>2} | {1:>8} | {2:>2}".format(dez, binaer, hexa)


x = 1
print(tabelle("Dezimal", "Binär", "Hexadezimal"))
while x <= 32:
    print(tabelle(x, bin(x), hex(x)))
    x += 1