"""
author: Markus Spitzer
file: easter.py
"""

import math

def easter(year):  
    a = year % 19
    b = year % 4
    c = year % 7
    k = year / 100
    p = (8 * k + 13) / 25
    q = k / 4
    M = (15 + k - p - q) % 30
    d = (19 * a + M) % 30
    N = (4 + k - q) % 7
    e = (2 * b + 4 * c + 6 * d + N) % 7

    if 22 + d + e <= 31:
        return "März", math.floor(22 + d + e)
    else:
        return "April", math.floor(d + e - 9)
