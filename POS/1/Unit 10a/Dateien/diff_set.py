"""
author: Markus Spitzer
file: diff_set.py
"""


def intersect(a, b):
    c = set({})

    for i in a:
        if i in b:
            c.add(i)

    return c
