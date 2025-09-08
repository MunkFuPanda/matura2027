"""
author: Markus Spitzer
file: testy.py
"""

i = 0
summe = 0

while i <= 100:
    if i % 2 == 0:
        summe += i

    i += 1


print(summe)
