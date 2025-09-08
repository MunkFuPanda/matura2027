"""
author: Markus Spitzer
file: nummer9.py
"""

t = ((False, False), (False, True), (True, False), (True, True))

for i in t:
    a = i[0]
    b = i[1]
    erg = a and b

    print("...")
