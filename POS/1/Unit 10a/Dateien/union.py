"""
author: Markus Spitzer
file: union.py
"""


def union(a, b):
    c = set({})

    for i in a:
        c.add(i)

    for i in b:
        c.add(i)

    return c
