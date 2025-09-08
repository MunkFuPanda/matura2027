"""
author: Markus Spitzer
file: is_divisable.py
"""

# ohne catch
def is_divisable(a, b):
    return (a % b == 0)

# mit catch
def is_divisable(a, b):
    try:
        return a % b == 0
    except:
        return False
