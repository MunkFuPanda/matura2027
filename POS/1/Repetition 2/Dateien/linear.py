"""
author: Markus Spitzer
file: linear.py
"""

import 2 as slope
import 3 as intercept

def slope(x1, y1, x2, y2):
    x = abs(x1 - x2)
    y = abs(y1 - y2)
    r = math.sqrt(x ** 2 + y ** 2)

    return r / x


def intercept(x1, y1, x2, y2):
    k = slope(x1, y1, x2, y2)

    return y1 - k * x1

