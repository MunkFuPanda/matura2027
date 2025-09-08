"""
author: Markus Spitzer
file: ueben.py
"""

#a
summe1 = 0
counter1 = 1
summandanzahl1 = 0

while summandanzahl1 <= 100:
    if counter1 % 2 == 0:
        summe1 += counter1
        summandanzahl1 += 1
    counter1 += 1

print(summe1)

#b
summe2 = 0
counter2 = 1
summandanzahl2 = 0

while summandanzahl2 <= 50:
    if counter2 % 2 == 1:
        summe2 += counter2
        summandanzahl2 += 1
    counter2 += 1

print(summe2)

#c
summe3 = 0
counter3 = 1
summandanzahl3 = 0

while summandanzahl3 <= 100:
    if counter3 % 2 == 0:
        summe3 += counter3
        summandanzahl3 += 1
    counter3 += 1

print(summe3 / summandanzahl3)


        
