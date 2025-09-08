"""
author: Markus Spitzer
file: arithmean3.py
"""

zahl = input("Geben Sie eine Zahl an: ")
counter = 0
summe = 0

while zahl != "q":
    counter += 1
    summe += int(zahl)
    zahl = input("Geben Sie eine Zahl an: ")

print(summe / counter)
