"""
author: Markus Spitzer
file: arithmean.py
"""

x = []
summe = 0

for Zahl in range(10):
     b = int(input("Geben Sie eine Zahl an: "))
     x.append(b)

for Zahl in x:
    summe += Zahl

print(summe / len(x)) 
    
    
