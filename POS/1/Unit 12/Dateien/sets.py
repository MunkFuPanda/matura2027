"""
author: Markus Spitzer
file: sets.py
"""


def minus(a, b):
    for i in b:
        a.discard(i)
        
    return a
