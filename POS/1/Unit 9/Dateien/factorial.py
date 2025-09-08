"""
author: Markus Spitzer
file: factorial.py
"""


def fak(n):
    erg = 1
    counter = 1
    while counter <= n:
        erg *= counter
        counter += 1
    return erg

