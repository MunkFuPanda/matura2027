"""
author: Markus Spitzer
file: egyptian_mul.py
"""

from math import floor

def egyptian_mul(a, b):
    summe = 0
    
    while a != 1:
        if a % 2 == 1:
            summe += b
        
        a = floor(a/2)
        b *= 2

    return summe + b
