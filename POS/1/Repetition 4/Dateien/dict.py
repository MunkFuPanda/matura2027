"""
author: Markus Spitzer
file: dict.py
"""


def sum_values(d):
    return sum(d.values())


def sub_values(d):
    return (lambda x: x[0] - sum(x[1:]))(list(d.values()))


def sort_values(d):
    return sorted(d.values())


def sort_values2(d):
    return sorted(d.values(), key=lambda x: x[1])
