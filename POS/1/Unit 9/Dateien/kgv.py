"""
author: Markus Spitzer
file: kgv.py
"""

import utils

a = int(input("Geben Sie die erste Zahl an: "))
b = int(input("Geben Sie die zweite Zahl an: "))
kgv = (a * b) / utils.ggt(a, b)

print(kgv)
