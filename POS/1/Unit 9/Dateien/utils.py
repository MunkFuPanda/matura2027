"""
author: Markus Spitzer
file: utils.py
"""

def ggt(a, b):
    while b != 0:
        h = a % b
        a = b
        b = h

    return a
    
