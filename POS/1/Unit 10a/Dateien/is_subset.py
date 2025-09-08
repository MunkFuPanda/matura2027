"""
author: Markus Spitzer
file: is_subset.py
"""


def is_subset(a, b):
    for i in a:
        if i not in b:
           return False

    return True
