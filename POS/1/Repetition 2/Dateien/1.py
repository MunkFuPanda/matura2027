"""
author: Markus Spitzer
file: 1.py
"""

import math


def compare(a, b):
    if a == b:
        return 0
    return -((a - b) / abs(a - b))


def slope(x1, y1, x2, y2):
    ergx = abs(x1 - x2)
    ergy = abs(y1 - y2)

    return math.sin(ergy / math.sqrt(ergx ** 2 + ergy ** 2))


#def intercept(x1, y1, x2, y2):
    
print(slope(1,1,2,3))
