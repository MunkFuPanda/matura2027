"""
author: Markus Spitzer
file: fibonacci.py
"""

x1 = 0
x2 = 1
counter = 0
n = int(input("Geben Sie einen Wert für n an: "))

while counter <= n:
    print(x2)
    
    xn = x1 + x2
    x1 = x2
    x2 = xn
    counter += 1

