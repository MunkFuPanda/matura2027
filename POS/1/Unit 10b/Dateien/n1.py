"""
author: Markus Spitzer
file: n1.py
"""

import random

name = input("Hallo! Wie lautet dein Name?\n")
zahl = random.randint(1,20)

print(f"""Servus {name}!
Ich denke mir jetzt eine Zahl zwischen 1 und 20 aus.
Wie lautet dein Tipp? 10
Deine Zahl ist zu hoch!
Wie lautet dein Tipp? 2
Deine Zahl ist zu niedrig!
Wie lautet dein Tipp? 4
Gut geraten, Maxi! Du hast meine Zahl in 3 Versuchen erraten!
""")

for i in range(6):
    guess = int(input("Dein Tipp:"))

    if guess == zahl:
        print(f"Gut geraten, {name}! Du hast meine Zahl in 3 Versuchen erraten!")
        break
    elif guess > zahl:
        print("Deine Zahl ist zu hoch!")
    elif guess < zahl:
        print("Deine Zahl ist zu niedrig!")
