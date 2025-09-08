"""
author: Markus Spitzer
file: boolsche_werte.py
"""

n = int(input("Geben Sie einen Wert für n an: "))
n1 = int(input("Geben Sie einen Wert für n1 an: "))
n2 = int(input("Geben Sie einen Wert für n2 an: "))
s = int(input("Geben Sie einen Wert für s an: "))

if n >=1 and n <=10:
    print("True")

elif s >= 5:
    print("True")

elif n1 < (n2 * 2):
    print("True")

elif n1 < (n2 / 2):
    print("False")

