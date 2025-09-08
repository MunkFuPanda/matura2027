"""
author: Markus Spitzer
file: e.py
"""

import factorial

n = int(input("Geben Sie n an: "))
counter = 1
e = 1

while counter <= n:
    e += 1 / factorial.fak(counter)
    counter += 1

print(e)

