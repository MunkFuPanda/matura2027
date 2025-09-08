"""
author: Markus Spitzer
file: pi.py
"""

import math

n = int(input("Genauigkeit:"))
my_pi = 0
positive = True

for i in range(1, n * 2, 2):
    if positive:
        my_pi += 4/i
    else:
        my_pi -= 4/i
    positive = not positive

print(f'My Pi: {my_pi} \n Difference: {abs(my_pi - math.pi)}')
