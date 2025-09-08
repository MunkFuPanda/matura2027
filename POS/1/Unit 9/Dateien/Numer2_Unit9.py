"""
author: Markus Spitzer
file: Nummer2_Unit9.py
"""

i = 1
count = 0
summe = 0

while count <= 100:
    if i % 2 == 0:
        count += 1
        summe += i
    i += 1

print(summe)

